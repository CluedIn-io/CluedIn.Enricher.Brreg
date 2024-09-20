using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.Brreg
{
    public class BrregExternalSearchJobData : CrawlJobData
    {
        public BrregExternalSearchJobData(IDictionary<string, object> configuration)
        {
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>();
        }
        
    }
}
