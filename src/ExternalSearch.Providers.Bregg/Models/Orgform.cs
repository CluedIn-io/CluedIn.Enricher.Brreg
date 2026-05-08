using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Orgform
	{
		[JsonPropertyName("kode")]
		public string Kode { get; set; }

		[JsonPropertyName("beskrivelse")]
		public string Beskrivelse { get; set; }
	}
}
