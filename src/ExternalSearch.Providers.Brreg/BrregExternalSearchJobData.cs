using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.Brreg;

// Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName
// Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode
// Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website
// Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg
public class BrregExternalSearchJobData : CrawlJobData
{
    public BrregExternalSearchJobData(IDictionary<string, object> configuration)
    {
        AcceptedEntityType = GetValue<string>(configuration, Constants.KeyName.AcceptedEntityType);
        CountryCodeKey = GetValue<string>(configuration, Constants.KeyName.CountryCodeKey);
        WebsiteKey = GetValue<string>(configuration, Constants.KeyName.WebsiteKey);
        BrregIdKey = GetValue<string>(configuration, Constants.KeyName.BrregIdKey);
    }

    public string AcceptedEntityType { get; }
    public string CountryCodeKey { get; }
    public string WebsiteKey { get; }
    public string BrregIdKey { get; }

    public IDictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { Constants.KeyName.AcceptedEntityType, AcceptedEntityType },
            { Constants.KeyName.CountryCodeKey, CountryCodeKey },
            { Constants.KeyName.WebsiteKey, WebsiteKey },
            { Constants.KeyName.BrregIdKey, BrregIdKey }
        };
    }
}