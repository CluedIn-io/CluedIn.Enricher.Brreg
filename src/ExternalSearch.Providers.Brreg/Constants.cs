using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public static class Constants
{
    public const string ComponentName = "Brreg";
    public const string ProviderName = "Brreg";
    public static readonly Guid ProviderId = new("fb23a770-5d9e-4763-91a7-2d81c3c5bcb9");

    public static string About { get; set; } = "Brreg is an enricher which provides information on Norwegian companies";
    public static string Icon { get; set; } = "Resources.brreg_logo.svg";
    public static string Domain { get; set; } = "https://www.brreg.no/";

    public static AuthMethods AuthMethods => new()
    {
        token = new[]
        {
            new Control
            {
                name = nameof(BrregExternalSearchJobData.AcceptedEntityType),
                displayName = "Entity Type",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregExternalSearchJobData.CountryCodeKey),
                displayName = "Country Code",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregExternalSearchJobData.WebsiteKey),
                displayName = "Website",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregExternalSearchJobData.BrregIdKey),
                displayName = "Organization Number",
                type = "text",
                isRequired = true
            }
        }
    };

    public static IEnumerable<Control> Properties { get; set; } = new List<Control>();

    public static Guide Guide { get; set; } = null;
    public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;

    public struct KeyName
    {
        public const string AcceptedEntityType = nameof(AcceptedEntityType);
        public const string CountryCodeKey = nameof(CountryCodeKey);
        public const string WebsiteKey = nameof(WebsiteKey);
        public const string BrregIdKey = nameof(BrregIdKey);
    }
}