// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrregTests.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the Brreg tests class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.ExternalSearch;
using CluedIn.ExternalSearch.Providers.Bregg;
using CluedIn.ExternalSearch.Providers.Bregg.Models;
using CluedIn.ExternalSearch.Providers.Bregg.Vocabularies;
using CluedIn.Testing.Base.ExternalSearch;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Xunit;
using TestContext = CluedIn.Testing.Base.Context.TestContext;

namespace ExternalSearch.Bregg.Integration.Tests
{
    public class BrregTests : BaseExternalSearchTest<BrregExternalSearchProvider>
    {
        [Fact]
        public void RestResponseContent_DeserializesWithNewtonsoftJson()
        {
            const string content = @"{
                'organisasjonsnummer': '981125096',
                'navn': 'NORDEA AKTUARTJENESTER NORGE AS',
                'organisasjonsform': {
                    'kode': 'AS',
                    'beskrivelse': 'Aksjeselskap'
                },
                '_links': {
                    'self': { 'href': 'https://data.brreg.no/enhetsregisteret/api/enheter/981125096' }
                },
                'registrertIMvaregisteret': true,
                'paategninger': [{
                    'infotype': 'KONT',
                    'tekst': 'Kontaktperson mangler',
                    'innfoertDato': '2024-12-19'
                }],
                'naeringskode1': {
                    'kode': '66.290',
                    'beskrivelse': 'Tjenester tilknyttet forsikringsvirksomhet'
                }
            }";

            var organization = JsonConvert.DeserializeObject<BrregOrganization>(content);

            Assert.Equal(981125096, organization.BrregNumber);
            Assert.Equal("NORDEA AKTUARTJENESTER NORGE AS", organization.Name);
            Assert.Equal("{\"kode\":\"AS\",\"beskrivelse\":\"Aksjeselskap\"}", organization.OrganisationType);
            Assert.Equal("{\"self\":{\"href\":\"https://data.brreg.no/enhetsregisteret/api/enheter/981125096\"}}", organization.Links);
            Assert.True(organization.RegistredImGoodsRegisterBool == true);
            Assert.Equal("66.290", organization.IndustryCode1.Code);
            Assert.Equal("Kontaktperson mangler", organization.Endorsements.Single().Text);
        }

        [Fact]
        public void SearchResponse_DeserializesObjectLinks()
        {
            const string content = @"{
                '_embedded': { 'enheter': [] },
                '_links': {
                    'first': { 'href': 'https://data.brreg.no/enhetsregisteret/api/enheter?page=0' },
                    'self': { 'href': 'https://data.brreg.no/enhetsregisteret/api/enheter?page=1' }
                },
                'page': { 'size': 30, 'totalElements': 315, 'totalPages': 11, 'number': 0 }
            }";

            var response = JsonConvert.DeserializeObject<RootBrregOrganization>(content);

            Assert.Equal("https://data.brreg.no/enhetsregisteret/api/enheter?page=0", response.Links["first"].Href);
            Assert.Equal(315, response.Page.TotalElements);
            Assert.Equal(11, response.Page.TotalPages);
            Assert.Equal(0, response.Page.PageNumber);
        }

        [Theory]
        [InlineData("981125096")]
        public void Id_ResultFound(string brregId)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, brregId);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name = "Brreg-" + brregId,
                EntityType = EntityType.Organization,
                OriginEntityCode = new EntityCode(EntityType.Organization, "brreg", brregId),
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("971227222")]
        public void Id_HasWebsite(string brregId)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, brregId);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name = "Brreg-" + brregId,
                EntityType = EntityType.Organization,
                OriginEntityCode = new EntityCode(EntityType.Organization, "brreg", brregId),
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
            var clue = clues.First().Decompress();

            Assert.True(clue.Details.Data.EntityData.Properties.ContainsKey(BrregVocabulary.Organization.Website));
        }

        [Theory]
        [InlineData("123456789")]
        public void Id_NotResultFound(string brregId)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, brregId);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.Never);
            Assert.Empty(clues);
        }

        [Theory]
        [InlineData("NETTO AS", "NO")]
        public void TestResultsFound(string name, string countryCode)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode,
                countryCode);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name             = name,
                EntityType       = EntityType.Organization,
                OriginEntityCode = new EntityCode(EntityType.Organization, "test", name),
                Properties       = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("NETTO AS")]
        public void NameOnly_ResultsFound(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                                                                          Name             = name,
                                                                          EntityType       = EntityType.Organization,
                                                                          OriginEntityCode = new EntityCode(EntityType.Organization, "test", name),
                                                                          Properties       = properties.Properties
                                                                      };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("NETTO", "http://netto.no")]
        public void WebsiteTldResultsFound(string name, string website)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website, website);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                                                                          Name             = name,
                                                                          EntityType       = EntityType.Organization,
                                                                          OriginEntityCode = new EntityCode(EntityType.Organization, "test", name),
                                                                          Properties       = properties.Properties
                                                                      };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("NETTO", "http://netto.com")]
        public void WebsiteTldNoResultsFound(string name, string website)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website, website);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                                                                          Name       = name,
                                                                          EntityType = EntityType.Organization,
                                                                          Properties = properties.Properties
                                                                      };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.Never);
            Assert.Empty(clues);
        }

        [Theory]
        [InlineData("NETTO")]
        public void TestMultipleMatchingResultsFound(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode, "NO");

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name             = name,
                EntityType       = EntityType.Organization,
                OriginEntityCode = new EntityCode(EntityType.Organization, "test", name),
                Properties       = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("Harry Sacks Holdings IVS")]
        public void TestNoResultsFound(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name       = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.Never);

            Assert.Empty(clues);
        }

        [Theory]
        [InlineData("NETTO")]
        public void TestNoCountryCode(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name       = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.Never);

            Assert.True(clues.Count == 0);
        }

        [Theory]
        [InlineData("NETTO AS", "AF")]
        [Trait("Category", "slow")]
        public void TestWrongCountryCode(string name, string countryCode)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode,
                countryCode);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name       = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommandAsync(It.IsAny<ProcessClueCommand>()), Times.Never);

            Assert.Empty(clues);
        }

        [Fact]
        public void HandleEmptyResponseTest()
        {
            var brregExternalSearchProvider = new BrregExternalSearchProvider();

            var brregOrganization = new BrregOrganization {
                BrregNumber = 0,
                Bankrupt = null,
                BusinessAddress = null,
                Name = null
            };

            var orgRoot = new RootBrregOrganization() {
                Embedded = new Unit() { Data = new List<BrregOrganization>() { brregOrganization } },
            };

            var query = new ExternalSearchQuery {
                ProviderId = new Guid("fb23a770-5d9e-4763-91a7-2d81c3c5bcb9"),
                QueryKey = "abc",
                EntityType = EntityType.Organization
            };

            var result = new ExternalSearchQueryResult<BrregOrganization>(query, brregOrganization);
            var dummyContext = new TestContext().Context;
            var dummyRequest = new DummyRequest();

            var clues = brregExternalSearchProvider.BuildClues(dummyContext, query, result, dummyRequest);

            Assert.True(clues == null);
        }

        [Theory]
        [InlineData("981125096")]
        public void Id_DeserializationTest(string brregId)
        {
            var client  = new RestClient("https://data.brreg.no/enhetsregisteret");
#if CLUEDIN_V50
            var request = new RestRequest($"api/enheter/{brregId}", Method.Get);
#else
            var request = new RestRequest($"api/enheter/{brregId}", Method.GET);
#endif

            var response = client.Execute(request);
            var organization = JsonConvert.DeserializeObject<BrregOrganization>(response.Content);

            Assert.IsType<BrregOrganization>(organization);
            Assert.NotNull(organization.OrganisationType);
            Assert.True(BrregExternalSearchProviderUtil.IsJson(organization.OrganisationType));
            Assert.True(BrregExternalSearchProviderUtil.IsJson(organization.Links));

            var fullValue = JsonConvert.DeserializeObject<OrganizationType>(organization.OrganisationType);
            Assert.IsType<OrganizationType>(fullValue);
            Assert.NotNull(fullValue.Links.Self.Href);

            var selfLink = JsonConvert.DeserializeObject<SelfLink>(organization.Links);
            Assert.NotNull(selfLink.Self.Href);
        }

        [Theory]
        [InlineData("barclays")]
        public void Name_DeserializationTest(string name)
        {
            var client = new RestClient("https://data.brreg.no/enhetsregisteret");
#if CLUEDIN_V50
            var request = new RestRequest($"api/enheter?page=0&size=30&navn={name}", Method.Get);
#else
            var request = new RestRequest($"api/enheter?page=0&size=30&navn={name}", Method.GET);
#endif

            var response = client.Execute(request);
            var root = JsonConvert.DeserializeObject<RootBrregOrganization>(response.Content);
            var barclayOffshoreServices = root.Embedded.Data.Single(e => e.BrregNumber == 924315563);

            Assert.Equal("Kontaktperson mangler. Virksomheten har fått pålegg om å melde manglende rolle", barclayOffshoreServices.Endorsements.Single().Text);
        }
    }
}
