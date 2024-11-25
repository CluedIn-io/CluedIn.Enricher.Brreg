using System;
using System.Collections.Generic;
using System.Linq;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.Bregg
{
    public static class Constants
    {
        public const string ComponentName = "Brreg";
        public const string ProviderName = "Brreg";
        public static readonly Guid ProviderId = new Guid("fb23a770-5d9e-4763-91a7-2d81c3c5bcb9");

        public static string About { get; set; } = "Brreg is an enricher which provides information on Norwegian companies";
        public static string Icon { get; set; } = "Resources.brreg_logo.svg";
        public static string Domain { get; set; } = "https://www.brreg.no/";
        public const string Instruction = """
            [
              {
                "type": "bulleted-list",
                "children": [
                  {
                    "type": "list-item",
                    "children": [
                      {
                        "text": "Add the entity type to specify the golden records you want to enrich. Only golden records belonging to that entity type will be enriched."
                      }
                    ]
                  },
                  {
                    "type": "list-item",
                    "children": [
                      {
                        "text": "Add the vocabulary keys to provide the input for the enricher to search for additional information. For example, if you provide the website vocabulary key for the Web enricher, it will use specific websites to look for information about companies. In some cases, vocabulary keys are not required. If you don't add them, the enricher will use default vocabulary keys."
                      }
                    ]
                  }
                ]
              }
            ]
            """;

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods()
        {
            Token = new List<Control>() {
                new()
                {
                    DisplayName = "Accepted Entity Type",
                    Type = "input",
                    IsRequired = true,
                    Name = nameof(BrregExternalSearchJobData.AcceptedEntityType),
                    Help = "The entity type that defines the golden records you want to enrich (e.g., /Organization)."
                },
                new()
                {
                    DisplayName = "Name Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = nameof(BrregExternalSearchJobData.NameVocabularyKey),
                    Help = "The vocabulary key that contains the names of companies you want to enrich (e.g., organization.name)."
                },
                new()
                {
                    DisplayName = "Country Code Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = nameof(BrregExternalSearchJobData.CountryCodeVocabularyKey),
                    Help = "The vocabulary key that contains the country codes of companies you want to enrich (e.g., organization.countrycode)."
                },
                new()
                {
                    DisplayName = "Website Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = nameof(BrregExternalSearchJobData.WebsiteVocabularyKey),
                    Help = "The vocabulary key that contains the websites of companies you want to enrich (e.g., organization.website)."
                },
                new()
                {
                    DisplayName = "Brreg Code Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = nameof(BrregExternalSearchJobData.BrregCodeVocabularyKey),
                    Help = "The vocabulary key that contains the Brreg codes of companies you want to enrich (e.g., organization.brregs)."
                },
                new()
                {
                    DisplayName = "Skip Entity Code Creation (Brreg Code)",
                    Type = "checkbox",
                    IsRequired = false,
                    Name =  nameof(BrregExternalSearchJobData.SkipEntityCodeCreation),
                    Help = "Toggle to control the creation of new entity codes using the Brreg code."
                }
            }
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>() {  };
        public static Guide Guide { get; set; } = new Guide
        {
            Instructions = Instruction
        };
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}