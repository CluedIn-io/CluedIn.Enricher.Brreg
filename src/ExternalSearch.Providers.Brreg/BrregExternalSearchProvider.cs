// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the BrregExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Providers.Brreg.Clients;
using CluedIn.ExternalSearch.Providers.Brreg.Models;
using CluedIn.ExternalSearch.Providers.Brreg.Vocabularies;
using EntityType = CluedIn.Core.Data.EntityType;
using ExecutionContext = CluedIn.Core.ExecutionContext;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public partial class BrregExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata,
    IConfigurableExternalSearchProvider
{
    public BrregExternalSearchProvider()
        : base(BrregConstants.ProviderId, Array.Empty<EntityType>())
    {
    }

    public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider)
    {
        return new List<EntityType> { new BrregJobData(config).AcceptedEntityType }.AsReadOnly();
    }

    public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request,
        IDictionary<string, object> config, IProvider provider)
    {
        var jobData = new BrregJobData(config);

        if (!Accepts(request.EntityMetaData.EntityType))
            yield break;

        var countryCode = request.QueryParameters.GetValue(
                new VocabularyKey(jobData.CountryCodeVocabularyKey), new HashSet<string>())
            .FirstOrDefault()?
            .Trim();

        if (string.IsNullOrEmpty(countryCode) &&
            !string.Equals(countryCode, "no", StringComparison.InvariantCultureIgnoreCase) &&
            !string.Equals(countryCode, "norway", StringComparison.InvariantCultureIgnoreCase))
            yield break;

        var brregId =
            request.QueryParameters.GetValue(
                    new VocabularyKey(jobData.BrregIdVocabularyKey),
                    new HashSet<string>())
                .FirstOrDefault();

        // if we have organization ID, use it
        if (!string.IsNullOrEmpty(brregId))
        {
            yield return new ExternalSearchQuery(
                this,
                request.EntityMetaData.EntityType,
                "brreg_id",
                brregId);
        }
        else
        {
            var organizationName =
                request.QueryParameters.GetValue(
                        new VocabularyKey(jobData.OrganizationNameVocabularyKey),
                        new HashSet<string>())
                    .FirstOrDefault();
            if (!string.IsNullOrEmpty(organizationName))
                yield return new ExternalSearchQuery(
                    this,
                    request.EntityMetaData.EntityType,
                    "organization_name",
                    organizationName);
        }
    }

    public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query,
        IDictionary<string, object> config, IProvider provider)
    {
        var brregClient = new BrregClient();

        if (query.QueryParameters.TryGetValue("brreg_id", out var brregIds) && brregIds.FirstOrDefault() is { } brregId)
        {
            var brregOrganization = brregClient.GetBrregOrganizationByOrganizationNumber(brregId);

            if (brregOrganization != null)
            {
                yield return new ExternalSearchQueryResult<BrregOrganization>(query, brregOrganization);
                yield break;
            }
        }

        if (!query.QueryParameters.TryGetValue("organization_name", out var organizationNames) ||
            organizationNames.FirstOrDefault() is not { } organizationName) yield break;

        foreach (var brregOrganization in brregClient
                     .GetBrregOrganizationsByName(organizationName)
                     .Where(x => string.Equals(x.Navn, organizationName, StringComparison.InvariantCultureIgnoreCase)))
        {
            yield return new ExternalSearchQueryResult<BrregOrganization>(query, brregOrganization);
            yield break;
        }
    }


    public IEnumerable<Clue>? BuildClues(ExecutionContext context, IExternalSearchQuery query,
        IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config,
        IProvider provider)
    {
        var resultItem = result.As<BrregOrganization>();

        if (string.IsNullOrEmpty(resultItem.Data.Organisasjonsnummer))
            return null;

        var code = new EntityCode(
            request.EntityMetaData.EntityType,
            CodeOrigin.CluedIn.CreateSpecific("brreg"),
            resultItem.Data.Organisasjonsnummer);

        var clue = new Clue(code, context.Organization);
        clue.Data.EntityData.Codes.Add(request.EntityMetaData.OriginEntityCode);

        clue.Data.EntityData.Name = request.EntityMetaData.Name;

        PopulateMetadata(clue.Data.EntityData, resultItem);

        return new[] { clue };
    }

    public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result,
        IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
    {
        return GetPrimaryEntityMetadata(context, result, request);
    }

    public string Icon { get; } = BrregConstants.Icon;
    public string Domain { get; } = BrregConstants.Domain;
    public string About { get; } = BrregConstants.About;

    public AuthMethods AuthMethods { get; } = BrregConstants.AuthMethods;
    public IntegrationType Type { get; } = BrregConstants.IntegrationType;

    public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context,
        IExternalSearchQueryResult result, IExternalSearchRequest request)
    {
        var metadata = new EntityMetadataPart
        {
            EntityType = request.EntityMetaData.EntityType,
            OriginEntityCode = request.EntityMetaData.OriginEntityCode
        };

        metadata.Codes.Add(metadata.OriginEntityCode);
        metadata.Codes.Add(request.EntityMetaData.OriginEntityCode);

        metadata.Name = request.EntityMetaData.Name;
        metadata.EntityType = request.EntityMetaData.EntityType;

        PopulateMetadata(metadata, result.As<BrregOrganization>());

        return metadata;
    }

    private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<BrregOrganization> resultItem)
    {
        // metadata.EntityType = EntityType.Organization;
        metadata.DisplayName = resultItem.Data.Navn;

        metadata.Properties[BrregVocabulary.Organization.BrregNumber] =
            resultItem.Data.Organisasjonsnummer.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.FoundedDate] =
            resultItem.Data.Stiftelsesdato.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.RegistrationDate] =
            resultItem.Data.RegistreringsdatoEnhetsregisteret.PrintIfAvailable();

        metadata.Properties[BrregVocabulary.Organization.OrganizationTypeFull] =
            resultItem.Data.Organisasjonsform.Beskrivelse.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.OrganizationType] =
            resultItem.Data.Organisasjonsform.Kode.PrintIfAvailable();


        // TODO: Save both addresses
        if (resultItem.Data.Postadresse != null)
            PopulateAddress(metadata, BrregVocabulary.Organization.Address, resultItem.Data.Postadresse);
        else
            PopulateAddress(metadata, BrregVocabulary.Organization.BusinessAddress, resultItem.Data.Forretningsadresse);

        metadata.Properties[BrregVocabulary.Organization.OrganizationTypeFull] =
            resultItem.Data.Organisasjonsform.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.VoluntaryRegistered] =
            resultItem.Data.RegistrertIFrivillighetsregisteret.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.RegisteredImGoodsRegister] =
            resultItem.Data.RegistrertIMvaregisteret.PrintIfAvailable();
        // TODO: ROK: Add these properties
        // metadata.Properties[BrregVocabulary.Organization.RegisteredBusinessRegister] =
        //      resultItem.Data.RegistredImGoodsRegisterBool.PrintIfAvailable();
        // metadata.Properties[BrregVocabulary.Organization.RegisteredFoundingRegister] =
        //     resultItem.Data.RegisteredFoundingRegisterBool.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.NumberEmployees] =
            resultItem.Data.AntallAnsatte.PrintIfAvailable();
        // metadata.Properties[BrregVocabulary.Organization.BankruptBool] =
        //     resultItem.Data.BankruptBool.PrintIfAvailable();
        // metadata.Properties[BrregVocabulary.Organization.UnderLiquidation] =
        //     resultItem.Data.UnderLiquidationBool.PrintIfAvailable();
        // metadata.Properties[BrregVocabulary.Organization.UnderLiquidationOrDissolution] =
        //     resultItem.Data.UnderLiquidationOrDissolutionBool.PrintIfAvailable();
        // metadata.Properties[BrregVocabulary.Organization.BrregUrl] = uri.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.IndustryCode] =
            resultItem.Data.Naeringskode1?.Kode.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.IndustryDescription] =
            resultItem.Data.Naeringskode1?.Beskrivelse.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.InstitutionSectorCode] =
            resultItem.Data.InstitusjonellSektorkode?.Kode.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.InstitutionSectorDescription] =
            resultItem.Data.InstitusjonellSektorkode?.Beskrivelse.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.LanguageVariant] =
            resultItem.Data.Maalform.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.LatestFiledAnnualAccounts] =
            resultItem.Data.SisteInnsendteAarsregnskap.PrintIfAvailable();
        metadata.Properties[BrregVocabulary.Organization.Website] = resultItem.Data.Hjemmeside.PrintIfAvailable();
    }

    private static void PopulateAddress(IEntityMetadata metadata, BrregAddressVocabulary vocabulary,
        Postadresse address)
    {
        metadata.Properties[vocabulary.CountryCode] = address.Landkode.PrintIfAvailable();
        metadata.Properties[vocabulary.PostalCode] = address.Postnummer.PrintIfAvailable();
        metadata.Properties[vocabulary.Country] = address.Land.PrintIfAvailable();
        metadata.Properties[vocabulary.Municipality] = address.Kommunenummer.PrintIfAvailable();
        metadata.Properties[vocabulary.MunicipalityNumber] = address.Kommunenummer.PrintIfAvailable();
        metadata.Properties[vocabulary.Address] =
            address.Adresse.PrintIfAvailable(v => string.Join(Environment.NewLine, v));
        metadata.Properties[vocabulary.PostalArea] = address.Poststed.PrintIfAvailable();
    }
}