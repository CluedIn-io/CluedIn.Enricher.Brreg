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

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods()
        {
            token = new List<Control>() {
                new()
                {
                    displayName = "Accepted Entity Type",
                    type = "input",
                    isRequired = true,
                    name = nameof(BrregExternalSearchJobData.AcceptedEntityType)
                },
                new()
                {
                    displayName = "Name Vocabulary Key",
                    type = "input",
                    isRequired = false,
                    name = nameof(BrregExternalSearchJobData.NameVocabularyKey)
                },
                new()
                {
                    displayName = "Country Code Vocabulary Key",
                    type = "input",
                    isRequired = false,
                    name = nameof(BrregExternalSearchJobData.CountryCodeVocabularyKey)
                },
                new()
                {
                    displayName = "Website Vocabulary Key",
                    type = "input",
                    isRequired = false,
                    name = nameof(BrregExternalSearchJobData.WebsiteVocabularyKey)
                },
                new()
                {
                    displayName = "Brreg Code Vocabulary Key",
                    type = "input",
                    isRequired = false,
                    name = nameof(BrregExternalSearchJobData.BrregCodeVocabularyKey)
                }
            }
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>() {  };
        public static Guide Guide { get; set; } = null;
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}