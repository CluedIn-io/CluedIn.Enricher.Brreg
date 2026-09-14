using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class IndustryCode1
	{
		[JsonProperty("kode")]
		public string Code { get; set; }

		[JsonProperty("beskrivelse")]
		public string Description { get; set; }
	}
}
