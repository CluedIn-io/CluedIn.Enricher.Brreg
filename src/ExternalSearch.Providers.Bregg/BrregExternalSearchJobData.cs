using System.Collections.Generic;
using CluedIn.Core.Crawling;
using static CluedIn.ExternalSearch.Providers.Bregg.Constants;

namespace CluedIn.ExternalSearch.Providers.Bregg
{
    public class BrregExternalSearchJobData : CrawlJobData
    {
        public BrregExternalSearchJobData(IDictionary<string, object> configuration)
        {
            AcceptedEntityType = GetValue(configuration, KeyName.AcceptedEntityType, default(string));
            NameVocabularyKey = GetValue(configuration, KeyName.NameVocabularyKey, default(string));
            CountryCodeVocabularyKey = GetValue(configuration, KeyName.CountryCodeVocabularyKey, default(string));
            WebsiteVocabularyKey = GetValue(configuration, KeyName.WebsiteVocabularyKey, default(string));
            BrregCodeVocabularyKey = GetValue(configuration, KeyName.BrregCodeVocabularyKey, default(string));
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { KeyName.AcceptedEntityType, AcceptedEntityType },
                { KeyName.NameVocabularyKey, NameVocabularyKey },
                { KeyName.CountryCodeVocabularyKey, CountryCodeVocabularyKey },
                { KeyName.WebsiteVocabularyKey, WebsiteVocabularyKey },
                { KeyName.BrregCodeVocabularyKey, BrregCodeVocabularyKey },
            };
        }

        public string AcceptedEntityType { get; set; }
        public string NameVocabularyKey { get; set; }
        public string CountryCodeVocabularyKey { get; set; }
        public string WebsiteVocabularyKey { get; set; }
        public string BrregCodeVocabularyKey { get; set; }
    }
}
