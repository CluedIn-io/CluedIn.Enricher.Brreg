using CluedIn.Core.Crawling;
using CluedIn.Core.Data.Vocabularies;
using EntityType = CluedIn.Core.Data.EntityType;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public class BrregJobData : CrawlJobData
{
    public BrregJobData(IDictionary<string, object> configuration)
    {
        var acceptedEntityType = GetValue<string>(configuration, BrregConstants.KeyName.AcceptedEntityType);
        AcceptedEntityType = string.IsNullOrEmpty(acceptedEntityType) ? acceptedEntityType : EntityType.Organization;

        var organizationNameKey = GetValue<string>(configuration, BrregConstants.KeyName.OrganizationNameKey);
        OrganizationNameVocabularyKey = string.IsNullOrEmpty(organizationNameKey)
            ? Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName
            : new VocabularyKey(organizationNameKey);

        var countryCodeKey = GetValue<string>(configuration, BrregConstants.KeyName.CountryCodeKey);
        CountryCodeVocabularyKey = string.IsNullOrEmpty(countryCodeKey)
            ? Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode
            : new VocabularyKey(countryCodeKey);

        var brregIdVocabularyKey = GetValue<string>(configuration, BrregConstants.KeyName.BrregIdKey);
        BrregIdVocabularyKey = string.IsNullOrEmpty(brregIdVocabularyKey)
            ? Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesBrreg
            : new VocabularyKey(brregIdVocabularyKey);
    }

    public string AcceptedEntityType { get; }
    public string OrganizationNameVocabularyKey { get; }
    public string CountryCodeVocabularyKey { get; }
    public string BrregIdVocabularyKey { get; }

    public IDictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { BrregConstants.KeyName.AcceptedEntityType, AcceptedEntityType },
            { BrregConstants.KeyName.OrganizationNameKey, OrganizationNameVocabularyKey },
            { BrregConstants.KeyName.CountryCodeKey, CountryCodeVocabularyKey },
            { BrregConstants.KeyName.BrregIdKey, BrregIdVocabularyKey }
        };
    }
}