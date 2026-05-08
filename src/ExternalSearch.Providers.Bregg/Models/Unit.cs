using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    public class Unit
    {
        [JsonPropertyName("enheter")]
        public List<BrregOrganization> Data { get; set; }
    }
}
