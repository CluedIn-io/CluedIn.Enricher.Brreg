using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class IndustryCode1
	{
		[JsonPropertyName("kode")]
		public string Code { get; set; }

		[JsonPropertyName("beskrivelse")]
		public string Description { get; set; }
	}
}
