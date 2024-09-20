using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models;

public class SelfLink
{
    [DeserializeAs(Name = "self")] public LinkHref Self { get; set; }
}