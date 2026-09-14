using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Link
	{
		[JsonProperty("rel")]
		public string Rel { get; set; }

		[JsonProperty("href")]
		public string Href { get; set; }
	}
}
