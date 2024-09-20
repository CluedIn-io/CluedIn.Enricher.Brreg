using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.Brreg
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
            token = new List<Control>()
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>() {  };

        public static Guide Guide { get; set; } = null;
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}