# Migrating an Enricher to Multi-Version Targeting

This document tracks the migration of `CluedIn.Enricher.Brreg` (published NuGet package
`CluedIn.Provider.ExternalSearch.Bregg` — the double-g is a pre-existing naming quirk, not a typo
introduced here) from a single-version build to the multi-version targeting pattern.

Prior art: `CluedIn.Crawling.MasterDataServices`, `CluedIn.Connector.AzureEventHubs`,
`CluedIn.Enricher.GoogleMaps`, and `CluedIn.Connector.Dataverse.V2`'s own
`docs/multi-version-targeting-migration.md` docs. GoogleMaps' doc corrected the older two on the
`multiVersionCluedInTargets` schema and the `.470`/`.480`/`.500` package-suffix convention — both
confirmed still accurate here.

Branch: `feature/multi-version-targeting` (off `origin/develop`).

---

## Overview

| CluedIn version | .NET TFM | Package suffix |
|---|---|---|
| 4.7.0 | net6.0 | `.470` |
| 4.8.0 | net6.0 | `.480` |
| 5.0.0-beta.* | net10.0 | `.500` |

Verified via throwaway restore against this repo's own `NuGet.config` feeds: `5.0.0-*` resolves to
`5.0.0-beta.575`. `4.7.0` and `4.8.0` both restore `CluedIn.ExternalSearch` cleanly against the
existing feeds — no `NuGet.config` changes needed beyond the casing fix below.

**4.6.0 excluded**, matching the MasterDataServices/GoogleMaps/Dataverse.V2 precedent — no evidence
in this repo's source of an API requiring it (this is an ExternalSearch provider, not a connector;
no `IStreamRepository` usage at all).

---

## Step 1 — Pipeline template (`azure-pipelines.yml`)

Switched from the single-version `crawler.build.yml` steps-template to `crawler.build.jobs.yml`.
Kept the existing `pool: vmImage: 'windows-latest'` (this repo was already on Windows agents, unlike
Dataverse.V2's `ubuntu-22.04` — preserved rather than changed, no reason found to switch it).
Dropped the explicit `UseDotNet@2` (each job in the jobs-template installs its own SDK per
`global.json`).

---

## Step 2 — `Directory.Build.props`

Honours `CluedInMultiVersionTargetFramework` (net10.0 local fallback), derives
`CLUEDIN_V47`/`V48`/`V50` `DefineConstants`. Also pinned `LangVersion` to `13.0` explicitly —
without it, the net6.0 leg failed with `CS8936` on `Constants.cs`'s raw string literal (net6.0's own
implied default is C# 10; the installed SDK compiles C# 13 down to net6.0 fine once told to). Same
trap GoogleMaps' doc flagged; this repo hit it for real.

---

## Step 3 — `Packages.props`

- Guarded `_CluedIn`.
- Added `_CluedInPackageSuffix` (dotless `Major.Minor.Patch`) and switched
  `CluedIn.Testing.Base` to the version-suffixed package ID
  (`CluedIn.Testing.Base.$(_CluedInPackageSuffix)`) — per GoogleMaps' finding, it publishes a
  separate package ID per CluedIn generation, not one package ID floated by version.
- Split `Microsoft.NET.Test.Sdk` / `xunit`&`xunit.v3` / `AutoFixture.Xunit2`&`3` conditionally on
  `CLUEDIN_V50` (this repo's `Packages.props` already had xunit v3 unconditionally — a net10.0-only
  setup before this migration).

---

## Step 4 — `NuGet.config`

Renamed `Nuget.config` → `NuGet.config` (two-step `git mv`, Windows case-insensitivity). Existing
feeds (`nuget.org`, `develop`, `release`, `AzurePipelines`) already sufficient for 4.7.0/4.8.0 —
verified via real restore, no `public` feed needed (unlike AzureEventHubs' repo).

---

## Step 5 — Test projects

- `test/Directory.Build.props`: stripped to just `IsTestProject` — was unconditionally including
  xunit v3 + AutoFixture.Xunit3 (would CS0433-clash with xunit v2 on the net6.0 legs).
- **Deleted `test/unit/Directory.Build.props`** — dead scaffold pinning ancient `Moq 4.5.30`/
  `Should 1.1.20` outside central package management; no `test/unit` csproj exists to consume it
  (same finding as GoogleMaps' repo).
- Added conditional `ItemGroup`s to
  `test/integration/ExternalSearch.Bregg.Integration.Tests.csproj`: xunit v3 + AutoFixture.Xunit3
  under `CLUEDIN_V50`, xunit v2 + AutoFixture.Xunit2 otherwise; `xunit.runner.visualstudio` in the
  common group (Packages.props already version-splits it). No `GlobalUsings.cs` needed — the one
  test file (`BrregTests.cs`) uses only a bare `using Xunit;` with no `ITestOutputHelper` or
  AutoFixture-attribute usage, so there's no namespace divergence to paper over.

---

## Step 6 — API compatibility audit across 4.7.0 / 4.8.0 / 5.0.0-beta.*

Built for real (`dotnet build -p:_CluedIn=<v> -p:CluedInMultiVersionTargetFramework=<tfm>`) against
all three legs plus the local default. Two real breaks found, both **not** in any prior migration
doc:

### RestSharp `Method.Get`/`RestResponse` (same class of break as GoogleMaps' repo)

`BrregExternalSearchProvider.cs` was written against RestSharp 114's API (`Method.Get` PascalCase,
`RestResponse` class). CluedIn 4.7/4.8 (net6.0) resolve RestSharp 106.x transitively (`Method.GET`
uppercase, `IRestResponse` interface — no common base with `RestResponse`). Fixed with:
- A `private const Method HttpGetMethod = ...` guarded by `#if CLUEDIN_V50`, used at all 4 call
  sites that previously wrote `Method.Get` directly (cleaner than repeating the guard 4 times).
- `#if`-guarding the `ConstructVerifyConnectionResponse` parameter type (`RestResponse` vs
  `IRestResponse`) — the `var`-inferred response locals at the call sites needed no guard.
- The integration test project (`BrregTests.cs`) had its own independent `Method.Get` call site,
  guarded the same way (test code isn't automatically covered by fixing `src/`).

### Nager.PublicSuffix major-version break (new finding — not RestSharp, not CluedIn.Core)

`CluedIn.Core`/`CluedIn.ExternalSearch` bring in `Nager.PublicSuffix` transitively (not an explicit
`PackageReference` in this repo at all), and it resolves very differently by CluedIn generation:
- CluedIn 4.7.0/4.8.0 (net6.0) → **Nager.PublicSuffix 2.4.0** — flat `Nager.PublicSuffix` namespace,
  no `RuleProviders`/`Exceptions` sub-namespaces, no `SimpleHttpRuleProvider` type at all (closest
  equivalent: `WebTldRuleProvider`, both ctor params defaulted so a no-arg call works),
  `DomainInfo.TLD` property.
- CluedIn 5.0.0-beta.* (net10.0) → **Nager.PublicSuffix 3.8.0** — split into
  `Nager.PublicSuffix.RuleProviders`/`Nager.PublicSuffix.Exceptions`, `SimpleHttpRuleProvider`
  exists, `DomainInfo.TopLevelDomain` (renamed from `TLD`, along with `Subdomain`/
  `FullyQualifiedDomainName`/`TopLevelDomainRule`).

Confirmed both API surfaces by loading the actual restored DLLs via PowerShell reflection
(`[System.Reflection.Assembly]::LoadFrom` + `GetProperties()`/`GetConstructors()`), not by guessing
from a changelog. Fixed in `src/ExternalSearch.Providers.Bregg/Net/DomainName.cs`:
`#if CLUEDIN_V50`-guarded the rule-provider construction and the imports, and added a
`DomainName.GetTopLevelDomain(DomainInfo)` compatibility helper so the one call site in
`BrregExternalSearchProvider.cs` (`domain.TopLevelDomain` → `DomainName.GetTopLevelDomain(domain)`)
doesn't need its own `#if`.

All four scenarios (4.7.0/net6.0, 4.8.0/net6.0, 5.0.0-beta.*/net10.0, and the local default) build
clean, 0 errors, after these fixes.

---

## Step 7 — Reset the semantic version (`GitVersion.yml`)

```yaml
next-version: 1.0
ignore:
  sha: []
  commits-before: 2026-06-20T00:00:00
```

Highest pre-existing tag is `4.6.2` at `2026-06-17T17:25:09+10:00` (checked every tag candidate's
real commit date via `git log -1 --format=%aI <tag>`, not tag-name sort order). Verified with the
pipeline's actual pinned `GitVersion.Tool 5.9.0` (installed to a scratch tool-path, not the
globally-installed version — which fails outright on this org's `pull-request: tag: pr` config on a
version mismatch): resolves to `1.0.0-multi-version-targeting.8x`, confirming the old tags are
correctly ignored — checked `MajorMinorPatch` explicitly (`1.0.0`), not just that the tool ran
without error.

**Widened to a 2-day buffer** (originally `2026-06-18T00:00:00`, one day past the tag): a sibling
migration in this same batch (`CluedIn.Enricher.Gleif`) found `GitVersion.Tool 5.9.0` parses
`commits-before` using the *local timezone of whatever machine runs it*, not UTC — and does so
**silently**, with no error, just a wrong version that keeps incrementing off the old 4.x tag. A
1-day buffer can be too tight depending on the CI agent's timezone; 2 days is safe regardless. This
repo's original 1-day value already re-verified as safe under every possible timezone offset (the
math: `commits-before` interpreted in any zone from UTC-12 to UTC+14 always lands after the tag's
actual UTC instant here), but widened anyway for consistency with the rest of the batch and as
defense against the same class of mistake elsewhere.

---

## Step 8 — Push and confirm CI

Status: see checklist below for outcome.

---

## Checklist

- [x] `azure-pipelines.yml` — switched to `crawler.build.jobs.yml` with `multiVersionCluedInTargets` (4.7.0, 4.8.0, 5.0.0-beta.*); kept existing `windows-latest` pool
- [x] `Directory.Build.props` — honours `CluedInMultiVersionTargetFramework` with net10.0 local fallback; `DefineConstants` derived; `LangVersion` pinned to 13.0 (real `CS8936` hit, not preventative)
- [x] `Packages.props` — `_CluedIn` guarded; `CluedIn.Testing.Base` switched to suffixed package ID; test packages split by `CLUEDIN_V50`
- [x] `NuGet.config` — renamed from `Nuget.config`; existing feeds confirmed sufficient
- [x] `test/Directory.Build.props` — stripped to `IsTestProject` only
- [x] `test/unit/Directory.Build.props` — deleted (dead scaffold, no project consumed it)
- [x] Integration test csproj — conditional xunit v2/v3 + AutoFixture selection
- [x] Source — RestSharp `Method.Get`/`RestResponse` guards (4 src call sites + 1 test call site); Nager.PublicSuffix 2.4.0↔3.8.0 guards in `DomainName.cs` (new finding, not in any prior doc)
- [x] `src`/tests build clean (0 errors) for all three legs, verified locally via real `dotnet build`
- [x] `GitVersion.yml` — `next-version: 1.0`; `ignore.commits-before: 2026-06-18T00:00:00`; verified with pinned GitVersion.Tool 5.9.0
- [ ] Push branch and confirm the actual Azure DevOps pipeline run is green end-to-end
