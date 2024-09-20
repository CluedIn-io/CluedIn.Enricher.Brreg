using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models;

public class Orgform
{
    [DeserializeAs(Name = "kode")] public string Kode { get; set; }

    [DeserializeAs(Name = "beskrivelse")] public string Beskrivelse { get; set; }
}