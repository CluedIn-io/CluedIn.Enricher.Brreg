using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Orgform
	{
		[JsonProperty("kode")]
		public string Kode { get; set; }

		[JsonProperty("beskrivelse")]
		public string Beskrivelse { get; set; }
	}
}
