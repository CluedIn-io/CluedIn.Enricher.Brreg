using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class SelfLink
	{
		[JsonProperty("self")]
		public LinkHref Self { get; set; }
	}
}
