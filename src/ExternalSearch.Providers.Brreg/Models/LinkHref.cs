using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models;

public class LinkHref
{
    [DeserializeAs(Name = "href")] public string Href { get; set; }
}