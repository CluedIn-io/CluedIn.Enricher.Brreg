using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class LinkHref
	{
		[JsonPropertyName("href")]
		public string Href { get; set; }
	}
}
