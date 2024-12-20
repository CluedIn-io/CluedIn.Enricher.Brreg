﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the BrregExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Provider;
using CluedIn.ExternalSearch.Providers.Bregg.Models;
using CluedIn.ExternalSearch.Providers.Bregg.Net;
using CluedIn.ExternalSearch.Providers.Bregg.Vocabularies;
using CluedIn.Processing.EntityResolution;
using RestSharp;
using EntityType = CluedIn.Core.Data.EntityType;

namespace CluedIn.ExternalSearch.Providers.Bregg
{
    /// <summary>The brreg external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class BrregExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata, IConfigurableExternalSearchProvider
    {
        private static readonly EntityType[] DefaultAcceptedEntityTypes = { EntityType.Organization };

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

            /// <summary>Initializes a new instance of the <see cref="BrregExternalSearchProvider"/> class.</summary>
        public BrregExternalSearchProvider()
            : base(Constants.ProviderId, DefaultAcceptedEntityTypes)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider) => this.Accepts(config);

        private IEnumerable<EntityType> Accepts(IDictionary<string, object> config)
        {
            if (config != null)
            {
                var breggExternalSearchJobData = new BrregExternalSearchJobData(config);
                if (!string.IsNullOrWhiteSpace(breggExternalSearchJobData.AcceptedEntityType))
                    return new EntityType[] { breggExternalSearchJobData.AcceptedEntityType };
            }

            // Fallback to default accepted entity types
            return DefaultAcceptedEntityTypes;
        }

        private bool Accepts(IDictionary<string, object> config, EntityType entityTypeToEvaluate)
        {
            var configurableAcceptedEntityTypes = this.Accepts(config).ToArray();

            return configurableAcceptedEntityTypes.Any(entityTypeToEvaluate.Is);
        }

        public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            if (!this.Accepts(config, request.EntityMetaData.EntityType))
                yield break;

            var existingResults = request.GetQueryResults<BrregOrganization>(this).ToList();

            bool NameFilter(string value) => OrganizationFilters.NameFilter(context, value);
            bool BrregFilter(string value) => existingResults.Any(r => string.Equals(r.Data.BrregNumber.ToString(CultureInfo.InvariantCulture), value, StringComparison.InvariantCultureIgnoreCase));

            var postFixes = new[] { "A/S", "AS", "ASA", "I/S", "IS", "K/S", "KS", "ENK", "ANS", "NUF", "P/S", "PS", "Enkeltpersonforetak", "Ansvarlig Selskap", "Aksjeselskap", "Norskregistrert utenlandsk foretak" }.Select(v => v.ToLowerInvariant()).ToHashSet();
            var contains = new[] { " no", "no ", "norway", "norge", "norsk", "æ", "ø", "å" }.Select(v => v.ToLowerInvariant()).ToHashSet();

            // Query Input.
            var entityType = request.EntityMetaData.EntityType;

            var breggExternalSearchJobData = new BrregExternalSearchJobData(config);

            var name = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, new HashSet<string>());
            var countryCode = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode, new HashSet<string>());
            var website = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website, new HashSet<string>());

            if (!string.IsNullOrWhiteSpace(breggExternalSearchJobData.NameVocabularyKey))
                name = request.QueryParameters.GetValue<string, HashSet<string>>(breggExternalSearchJobData.NameVocabularyKey, new HashSet<string>());

            if (!string.IsNullOrWhiteSpace(breggExternalSearchJobData.CountryCodeVocabularyKey))
                countryCode = request.QueryParameters.GetValue<string, HashSet<string>>(breggExternalSearchJobData.CountryCodeVocabularyKey, new HashSet<string>());

            if (!string.IsNullOrWhiteSpace(breggExternalSearchJobData.WebsiteVocabularyKey))
                website = request.QueryParameters.GetValue<string, HashSet<string>>(breggExternalSearchJobData.WebsiteVocabularyKey, new HashSet<string>());

            bool CountryFilter(string c) => c.Equals("no", StringComparison.OrdinalIgnoreCase)
                                         || c.Equals("NOR", StringComparison.OrdinalIgnoreCase)
                                         || c.Equals("Norway", StringComparison.OrdinalIgnoreCase)
                                         || c.Equals("Norge", StringComparison.OrdinalIgnoreCase);

            var namePostFixFilter = BrregExternalSearchProviderUtil.NamePostFixFilter(countryCode, CountryFilter, contains, name, postFixes);

            if (countryCode != null && countryCode.Any(CountryFilter))
            {
                namePostFixFilter = value => false;
            }

            var brregId = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, new HashSet<string>());

            if (!string.IsNullOrWhiteSpace(breggExternalSearchJobData.BrregCodeVocabularyKey))
                brregId = request.QueryParameters.GetValue<string, HashSet<string>>(breggExternalSearchJobData.BrregCodeVocabularyKey, new HashSet<string>());

            if (brregId != null && brregId.Any())
            {
                var values = brregId;

                foreach (var value in values.Where(v => !BrregFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier, value);
            }

            if (website != null && website.Any())
            {
                var hosts = website.Where(UriUtility.IsValid).Select(u => new Uri(u).Host.ToLowerInvariant()).Distinct();

                if (hosts.Any(h => DomainName.TryParse(h, out var domain) && string.Equals(domain.TLD, "no", StringComparison.InvariantCultureIgnoreCase)))
                    namePostFixFilter = value => false;
            }

            if (name != null && name.Any())
            {
                var values = name;

                foreach (var value in values.Where(v => !NameFilter(v) && !namePostFixFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
            }
        }

        public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query, IDictionary<string, object> config, IProvider provider)
        {
            var client = new RestClient("http://data.brreg.no/enhetsregisteret");
            RestRequest request;

            if (query.QueryParameters.ContainsKey(ExternalSearchQueryParameter.Identifier))
            {
                var id = query.QueryParameters[ExternalSearchQueryParameter.Identifier].FirstOrDefault();

                request = new RestRequest($"api/enheter/{id}", Method.GET) {
                    OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                };

                var response = client.Execute<BrregOrganization>(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        if (response.Data != null)
                        {
                            var org = response.Data;

                            if (org.BrregNumber == 0 && string.IsNullOrEmpty(org.Name))
                                yield break;

                            yield return new ExternalSearchQueryResult<BrregOrganization>(query, org);
                        }

                        break;
                    }
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.NotFound:
                        yield break;
                    default:
                    {
                        if (response.ErrorException != null)
                            throw new AggregateException(response.ErrorException.Message, response.ErrorException);
                        else
                            throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);
                    }
                }
            }

            if (query.QueryParameters.ContainsKey(ExternalSearchQueryParameter.Name))
            {
                var name = query.QueryParameters[ExternalSearchQueryParameter.Name].FirstOrDefault();
                if (!string.IsNullOrEmpty(name))
                {
                    request = new RestRequest($"api/enheter?page=0&size=30&navn={name}", Method.GET) {
                        OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                    };

                    var response = client.Execute<RootBrregOrganization>(request);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        {
                            if (response.Data?.Embedded?.Data != null)
                            {
                                var filterResponse = response.Data.Embedded.Data.Where(e => e.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
                                foreach (var org in filterResponse)
                                {
                                    if (org.BrregNumber == 0 && string.IsNullOrEmpty(org.Name))
                                        continue;

                                    yield return new ExternalSearchQueryResult<BrregOrganization>(query, org);
                                }
                            }

                            break;
                        }
                        case HttpStatusCode.NoContent:
                        case HttpStatusCode.NotFound:
                            yield break;
                        default:
                        {
                            if (response.ErrorException != null)
                                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
                            else
                                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);
                        }
                    }
                }
            }
        }

        public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<BrregOrganization>();

            if (resultItem.Data.BrregNumber == 0)
                return null;

            var clue = new Clue(request.EntityMetaData.OriginEntityCode, context.Organization);
            PopulateMetadata(clue.Data.EntityData, resultItem, request, config);

            return new[] { clue };
        }

        public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<BrregOrganization>();
            return CreateMetadata(resultItem, request, config);
        }

        public override IPreviewImage GetPrimaryEntityPreviewImage(
            ExecutionContext context,
            IExternalSearchQueryResult result,
            IExternalSearchRequest request)
        {
            // Note: This needs to be cleaned up, but since config and provider is not used in GetPrimaryEntityMetadata this is fine.
            var dummyConfig = new Dictionary<string, object>();
            var dummyProvider = new DefaultExternalSearchProviderProvider(context.ApplicationContext, this);

            return GetPrimaryEntityPreviewImage(context, result, request, dummyConfig, dummyProvider);
        }

        public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return null;
        }

        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<BrregOrganization> resultItem, IExternalSearchRequest request, IDictionary<string, object> config)
        {
            var metadata = new EntityMetadataPart();

            PopulateMetadata(metadata, resultItem, request, config);

            return metadata;
        }

        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<BrregOrganization> resultItem, IExternalSearchRequest request)
        {
            return new EntityCode(request.EntityMetaData.EntityType, GetCodeOrigin(), resultItem.Data.BrregNumber);
        }

        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("brreg");
        }

        public void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<BrregOrganization> resultItem, IExternalSearchRequest request, IDictionary<string, object> config)
        {
            var jobData = new BrregExternalSearchJobData(config);
            var code = request.EntityMetaData.OriginEntityCode;

            metadata.EntityType       = request.EntityMetaData.EntityType;
            metadata.Name             = request.EntityMetaData.Name;
            metadata.OriginEntityCode = code;

            if (!jobData.SkipEntityCodeCreation)
            {
                metadata.Codes.Add(GetOriginEntityCode(resultItem, request));
            }

            Uri uri = null;

            if (!string.IsNullOrEmpty(resultItem.Data.Links) && BrregExternalSearchProviderUtil.IsJson(resultItem.Data.Links))
            {
                if (BrregExternalSearchProviderUtil.CanDeserializeTo<SelfLink>(resultItem.Data.Links))
                {
                    var selfLink = JsonUtility.Deserialize<SelfLink>(resultItem.Data.Links);

                    if (selfLink.Self.Href != null)
                        uri = new Uri(selfLink.Self.Href);
                }
                else if (BrregExternalSearchProviderUtil.CanDeserializeTo<List<Link>>(resultItem.Data.Links))
                {
                    var linkList = JsonUtility.Deserialize<List<Link>>(resultItem.Data.Links);

                    var link = linkList?.FirstOrDefault(p => p.Rel == "self");

                    if (link != null)
                        uri = new Uri(link.Href);
                }
            }

            if (uri != null)
                metadata.Uri = uri;

            metadata.Properties[BrregVocabulary.Organization.BrregNumber]      = resultItem.Data.BrregNumber.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.FoundedDate]      = resultItem.Data.FoundedDate.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.RegistrationDate] = resultItem.Data.RegistrationDate.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.OrganizationType] = resultItem.Data.OrganisationType.PrintIfAvailable();

            if (resultItem.Data.OrganisationType != null && BrregExternalSearchProviderUtil.IsJson(resultItem.Data.OrganisationType))
            {
                var fullValue = JsonUtility.Deserialize<OrganizationType>(resultItem.Data.OrganisationType);

                metadata.Properties[BrregVocabulary.Organization.OrganizationTypeFull] = fullValue.FullNameForCode;
                metadata.Properties[BrregVocabulary.Organization.OrganizationType]     = fullValue.Code;
            }
            else
                metadata.Properties[BrregVocabulary.Organization.OrganizationType] = resultItem.Data.OrganisationType.PrintIfAvailable();

            if (resultItem.Data.PostAddress != null)
                PopulateAddress(metadata, BrregVocabulary.Organization.Address, resultItem.Data.PostAddress);

            if ((resultItem.Data.BusinessAddress != null) && resultItem.Data.BusinessAddress != resultItem.Data.PostAddress)
                PopulateAddress(metadata, BrregVocabulary.Organization.BusinessAddress, resultItem.Data.BusinessAddress);

            metadata.Properties[BrregVocabulary.Organization.OrganizationTypeFull]          = resultItem.Data.OrganisationType.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.VoluntaryRegistered]           = resultItem.Data.VoluntaryRegisteredBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.RegisteredImGoodsRegister]     = resultItem.Data.RegistredImGoodsRegisterBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.RegisteredBusinessRegister]    = resultItem.Data.RegistredImGoodsRegisterBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.RegisteredFoundingRegister]    = resultItem.Data.RegisteredFoundingRegisterBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.NumberEmployees]               = resultItem.Data.NumberEmployees.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.BankruptBool]                  = resultItem.Data.BankruptBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.UnderLiquidation]              = resultItem.Data.UnderLiquidationBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.UnderLiquidationOrDissolution] = resultItem.Data.UnderLiquidationOrDissolutionBool.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.BrregUrl]                      = uri.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.IndustryCode]                  = resultItem.Data.IndustryCode1?.Code.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.IndustryDescription]           = resultItem.Data.IndustryCode1?.Description.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.InstitutionSectorCode]         = resultItem.Data.InstitutionSectorCode?.Code.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.InstitutionSectorDescription]  = resultItem.Data.InstitutionSectorCode?.Description.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.LanguageVariant]               = resultItem.Data.LanguageVariant.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.LatestFiledAnnualAccounts]     = resultItem.Data.LatestFiledAnnualAccounts.PrintIfAvailable();
            metadata.Properties[BrregVocabulary.Organization.Website]                       = resultItem.Data.Website.PrintIfAvailable();
        }

        private static void PopulateAddress(IEntityMetadata metadata, BrregAddressVocabulary vocabulary, PostAddress address)
        {
            metadata.Properties[vocabulary.CountryCode]        = address.CountryCode.PrintIfAvailable();
            metadata.Properties[vocabulary.PostalCode]         = address.PostalCode.PrintIfAvailable();
            metadata.Properties[vocabulary.Country]            = address.Country.PrintIfAvailable();
            metadata.Properties[vocabulary.Municipality]       = address.Municipality.PrintIfAvailable();
            metadata.Properties[vocabulary.MunicipalityNumber] = address.MunicipalityNumber.PrintIfAvailable();
            metadata.Properties[vocabulary.Address]            = address.Address.PrintIfAvailable(v => string.Join(Environment.NewLine, v));
            metadata.Properties[vocabulary.PostalArea]         = address.PostalArea.PrintIfAvailable();
        }

        // Since this is a configurable external search provider, theses methods should never be called
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request) => BuildQueries(context, request, null, null).AsEnumerable();
        public override bool Accepts(EntityType entityType) => throw new NotSupportedException();
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query) => throw new NotSupportedException();
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request) => BuildClues(context, query, result, request, null, null);
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request) => GetPrimaryEntityMetadata(context, result, request, null, null);

        public string Icon { get; } = Constants.Icon;
        public string Domain { get; } = Constants.Domain;
        public string About { get; } = Constants.About;

        public AuthMethods AuthMethods { get; } = Constants.AuthMethods;
        public IEnumerable<Control> Properties { get; } = Constants.Properties;
        public Guide Guide { get; } = Constants.Guide;
        public IntegrationType Type { get; } = Constants.IntegrationType;
    }
}