using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Link
	{
		[JsonPropertyName("rel")]
		public string Rel { get; set; }

		[JsonPropertyName("href")]
		public string Href { get; set; }
	}
}
