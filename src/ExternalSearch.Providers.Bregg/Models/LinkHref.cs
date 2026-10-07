using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class LinkHref
	{
		[JsonProperty("href")]
		public string Href { get; set; }
	}
}
