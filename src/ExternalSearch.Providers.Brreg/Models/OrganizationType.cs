using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models;

public class OrganizationType
{
    [JsonProperty("kode")] public string Code { get; set; }

    [JsonProperty("beskrivelse")] public string FullNameForCode { get; set; }

    [JsonProperty("links")] public SelfLink Links { get; set; }
}