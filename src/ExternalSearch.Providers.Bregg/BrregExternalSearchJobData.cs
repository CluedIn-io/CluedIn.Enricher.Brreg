
using System.Collections.Generic;
using CluedIn.Core.Crawling;
using CluedIn.ExternalSearch.Providers.Bregg;

namespace CluedIn.ExternalSearch.Providers.Brreg
{
    public class BrregExternalSearchJobData : CrawlJobData
    {
        public BrregExternalSearchJobData(IDictionary<string, object> configuration)
        {
            ApiToken = GetValue<string>(configuration, Constants.KeyName.ApiToken);
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object> {
                { Constants.KeyName.ApiToken, ApiToken }
            };
        }

        public string ApiToken { get; set; }
    }
}
