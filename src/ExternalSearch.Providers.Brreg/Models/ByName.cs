using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Embedded
{
    [JsonProperty("enheter")] public List<BrregOrganization> Enheter { get; set; }
}

public class BrregOrganization
{
    [JsonProperty("organisasjonsnummer")] public string Organisasjonsnummer { get; set; }

    [JsonProperty("navn")] public string Navn { get; set; }

    [JsonProperty("organisasjonsform")] public Organisasjonsform Organisasjonsform { get; set; }

    [JsonProperty("registreringsdatoEnhetsregisteret")]
    public string RegistreringsdatoEnhetsregisteret { get; set; }

    [JsonProperty("registrertIMvaregisteret")]
    public bool? RegistrertIMvaregisteret { get; set; }

    [JsonProperty("naeringskode1")] public Naeringskode1 Naeringskode1 { get; set; }

    [JsonProperty("harRegistrertAntallAnsatte")]
    public bool? HarRegistrertAntallAnsatte { get; set; }

    [JsonProperty("forretningsadresse")] public Postadresse Forretningsadresse { get; set; }

    [JsonProperty("stiftelsesdato")] public string Stiftelsesdato { get; set; }

    [JsonProperty("institusjonellSektorkode")]
    public InstitusjonellSektorkode InstitusjonellSektorkode { get; set; }

    [JsonProperty("registrertIForetaksregisteret")]
    public bool? RegistrertIForetaksregisteret { get; set; }

    [JsonProperty("registrertIStiftelsesregisteret")]
    public bool? RegistrertIStiftelsesregisteret { get; set; }

    [JsonProperty("registrertIFrivillighetsregisteret")]
    public bool? RegistrertIFrivillighetsregisteret { get; set; }

    [JsonProperty("konkurs")] public bool? Konkurs { get; set; }

    [JsonProperty("underAvvikling")] public bool? UnderAvvikling { get; set; }

    [JsonProperty("underTvangsavviklingEllerTvangsopplosning")]
    public bool? UnderTvangsavviklingEllerTvangsopplosning { get; set; }

    [JsonProperty("maalform")] public string Maalform { get; set; }

    [JsonProperty("aktivitet")] public List<string> Aktivitet { get; set; }

    [JsonProperty("_links")] public Links Links { get; set; }

    [JsonProperty("hjemmeside")] public string Hjemmeside { get; set; }

    [JsonProperty("postadresse")] public Postadresse? Postadresse { get; set; }

    [JsonProperty("vedtektsdato")] public string Vedtektsdato { get; set; }

    [JsonProperty("vedtektsfestetFormaal")]
    public List<string> VedtektsfestetFormaal { get; set; }

    [JsonProperty("frivilligMvaRegistrertBeskrivelser")]
    public List<string> FrivilligMvaRegistrertBeskrivelser { get; set; }

    [JsonProperty("antallAnsatte")] public int? AntallAnsatte { get; set; }

    [JsonProperty("sisteInnsendteAarsregnskap")]
    public string SisteInnsendteAarsregnskap { get; set; }
}

public class First
{
    [JsonProperty("href")] public string Href { get; set; }
}

public class Forretningsadresse
{
    [JsonProperty("land")] public string Land { get; set; }

    [JsonProperty("landkode")] public string Landkode { get; set; }

    [JsonProperty("postnummer")] public string Postnummer { get; set; }

    [JsonProperty("poststed")] public string Poststed { get; set; }

    [JsonProperty("adresse")] public List<string> Adresse { get; set; }

    [JsonProperty("kommune")] public string Kommune { get; set; }

    [JsonProperty("kommunenummer")] public string Kommunenummer { get; set; }
}

public class InstitusjonellSektorkode
{
    [JsonProperty("kode")] public string Kode { get; set; }

    [JsonProperty("beskrivelse")] public string Beskrivelse { get; set; }
}

public class Last
{
    [JsonProperty("href")] public string Href { get; set; }
}

public class Links
{
    [JsonProperty("self")] public Self Self { get; set; }

    [JsonProperty("first")] public First First { get; set; }

    [JsonProperty("next")] public Next Next { get; set; }

    [JsonProperty("last")] public Last Last { get; set; }
}

public class Naeringskode1
{
    [JsonProperty("kode")] public string Kode { get; set; }

    [JsonProperty("beskrivelse")] public string Beskrivelse { get; set; }
}

public class Next
{
    [JsonProperty("href")] public string Href { get; set; }
}

public class Organisasjonsform
{
    [JsonProperty("kode")] public string Kode { get; set; }

    [JsonProperty("beskrivelse")] public string Beskrivelse { get; set; }

    [JsonProperty("_links")] public Links Links { get; set; }
}

public class Page
{
    [JsonProperty("size")] public int? Size { get; set; }

    [JsonProperty("totalElements")] public int? TotalElements { get; set; }

    [JsonProperty("totalPages")] public int? TotalPages { get; set; }

    [JsonProperty("number")] public int? Number { get; set; }
}

public class Postadresse
{
    [JsonProperty("land")] public string Land { get; set; }

    [JsonProperty("landkode")] public string Landkode { get; set; }

    [JsonProperty("postnummer")] public string Postnummer { get; set; }

    [JsonProperty("poststed")] public string Poststed { get; set; }

    [JsonProperty("adresse")] public List<string> Adresse { get; set; }

    [JsonProperty("kommune")] public string Kommune { get; set; }

    [JsonProperty("kommunenummer")] public string Kommunenummer { get; set; }
}

public class Root
{
    [JsonProperty("_embedded")] public Embedded Embedded { get; set; }

    [JsonProperty("_links")] public Links Links { get; set; }

    [JsonProperty("page")] public Page Page { get; set; }
}

public class Self
{
    [JsonProperty("href")] public string Href { get; set; }
}