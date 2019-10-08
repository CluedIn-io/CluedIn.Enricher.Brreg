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
using CluedIn.Testing.Base.Context;
using CluedIn.Testing.Base.ExternalSearch;
using Moq;
using RestSharp;
using Xunit;

namespace ExternalSearch.Bregg.Integration.Tests
{
    public class BrregTests : BaseExternalSearchTest<BrregExternalSearchProvider>
    {
        [Theory]
        [InlineData("981125096")]
        public void Id_ResultFound(string brregId)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, brregId);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory]
        [InlineData("971227222")]
        public void Id_HasWebsite(string brregId)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg, brregId);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
            Assert.Empty(clues);
        }

        [Theory(Skip = "Failed Mock exception. GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        [InlineData("NETTO AS", "NO")]
        public void TestResultsFound(string name, string countryCode)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode,
                countryCode);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name       = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory(Skip = "Failed Mock exception. GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        [InlineData("NETTO AS")]
        public void NameOnly_ResultsFound(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                                                                          Name       = name,
                                                                          EntityType = EntityType.Organization,
                                                                          Properties = properties.Properties
                                                                      };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.NotEmpty(clues);
        }

        [Theory(Skip = "Failed Mock exception. GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        [InlineData("NETTO", "http://netto.no")]
        public void WebsiteTldResultsFound(string name, string website)
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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
            Assert.Empty(clues);
        }

        [Theory(Skip = "Failed Mock exception. GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        [InlineData("NETTO")]
        public void TestMultipleMatchingResultsFound(string name)
        {
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, name);
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode, "NO");

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name       = name,
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            Setup(null, entityMetadata);

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.True(clues.Count > 1);
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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);

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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);

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

            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);

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
                Data = new List<BrregOrganization>() { brregOrganization },
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

        // TODO: Add tests for deserializing brreg results
        //       GetBy id vs search by name have subtle differences in the json

        [Theory]
        [InlineData("981125096")]
        public void Id_DeserializationTest(string brregId)
        {
            var client  = new RestClient("http://data.brreg.no/enhetsregisteret");
            var request = new RestRequest($"api/enheter/{brregId}", Method.GET);

            var response = client.Execute<BrregOrganization>(request);

            Assert.IsType<BrregOrganization>(response.Data);
            Assert.NotNull(response.Data.OrganisationType);
            Assert.True(BrregExternalSearchProviderUtil.IsJson(response.Data.OrganisationType));

            var fullValue = JsonUtility.Deserialize<OrganizationType>(response.Data.OrganisationType);
            Assert.IsType<OrganizationType>(fullValue);
        }
    }
}