using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class SelfLink
	{
		[JsonPropertyName("self")]
		public LinkHref Self { get; set; }
	}
}
