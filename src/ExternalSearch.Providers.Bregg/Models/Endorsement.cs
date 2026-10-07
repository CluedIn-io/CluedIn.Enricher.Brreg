using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
    public class Endorsement
    {
        [JsonProperty("infotype")]
        public string InformationType { get; set; }

        [JsonProperty("tekst")]
        public string Text { get; set; }

        [JsonProperty("innfoertDato")]
        public string IntroducedDate { get; set; }
    }
}
