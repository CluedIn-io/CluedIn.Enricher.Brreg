using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    public class Unit
    {
        [JsonProperty("enheter")]
        public List<BrregOrganization> Data { get; set; }
    }
}
