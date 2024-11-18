using RestSharp.Deserializers;
using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    public class Unit
    {
        [DeserializeAs(Name = "enheter")]
        public List<BrregOrganization> Data { get; set; }
    }
}
