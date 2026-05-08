using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Page
	{
		[JsonPropertyName("size")]
		public int Size { get; set; }

		[JsonPropertyName("page")]
		public int PageCount { get; set; }
	}
}
