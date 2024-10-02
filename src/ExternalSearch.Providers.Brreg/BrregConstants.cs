using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public static class BrregConstants
{
    public const string ComponentName = "Brreg";
    public const string ProviderName = "Brreg";
    public static readonly Guid ProviderId = new("fb23a770-5d9e-4763-91a7-2d81c3c5bcb9");

    public static string About { get; set; } = "Brreg is an enricher which provides information on Norwegian companies";
    public static string Icon { get; set; } = "Resources.brreg_logo.svg";
    public static string Domain { get; set; } = "https://brreg.no/";

    public static AuthMethods AuthMethods => new()
    {
        token = new[]
        {
            new Control
            {
                name = nameof(BrregJobData.AcceptedEntityType),
                displayName = "Entity Type",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregJobData.OrganizationNameVocabularyKey),
                displayName = "Organization Name Vocabulary Key",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregJobData.CountryCodeVocabularyKey),
                displayName = "Country Code Vocabulary Key",
                type = "text",
                isRequired = true
            },
            new Control
            {
                name = nameof(BrregJobData.BrregIdVocabularyKey),
                displayName = "Organization Number Vocabulary Key",
                type = "text",
                isRequired = true
            }
        }
    };

    public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;

    public struct KeyName
    {
        public const string AcceptedEntityType = nameof(AcceptedEntityType);
        public const string OrganizationNameKey = nameof(OrganizationNameKey);
        public const string CountryCodeKey = nameof(CountryCodeKey);
        public const string BrregIdKey = nameof(BrregIdKey);
    }
}