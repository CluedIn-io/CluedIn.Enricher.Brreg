using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.Bregg
{
    public class BrregExternalSearchJobData : CrawlJobData
    {
        public BrregExternalSearchJobData(IDictionary<string, object> configuration)
        {
            AcceptedEntityType = GetValue(configuration, nameof(AcceptedEntityType), default(string));
            NameVocabularyKey = GetValue(configuration, nameof(NameVocabularyKey), default(string));
            CountryCodeVocabularyKey = GetValue(configuration, nameof(CountryCodeVocabularyKey), default(string));
            WebsiteVocabularyKey = GetValue(configuration, nameof(WebsiteVocabularyKey), default(string));
            BrregCodeVocabularyKey = GetValue(configuration, nameof(BrregCodeVocabularyKey), default(string));
            SkipEntityCodeCreation = GetValue(configuration, nameof(SkipEntityCodeCreation), default(bool));
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { nameof(AcceptedEntityType), AcceptedEntityType },
                { nameof(NameVocabularyKey), NameVocabularyKey },
                { nameof(CountryCodeVocabularyKey), CountryCodeVocabularyKey },
                { nameof(WebsiteVocabularyKey), WebsiteVocabularyKey },
                { nameof(BrregCodeVocabularyKey), BrregCodeVocabularyKey },
                { nameof(SkipEntityCodeCreation), SkipEntityCodeCreation },
            };
        }

        public string AcceptedEntityType { get; set; }
        public string NameVocabularyKey { get; set; }
        public string CountryCodeVocabularyKey { get; set; }
        public string WebsiteVocabularyKey { get; set; }
        public string BrregCodeVocabularyKey { get; set; }
        public bool SkipEntityCodeCreation { get; set; }
    }
}
