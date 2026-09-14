using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Page
	{
		[JsonProperty("size")]
		public int Size { get; set; }

		[JsonProperty("number")]
		public int PageNumber { get; set; }

		[JsonProperty("totalElements")]
		public long TotalElements { get; set; }

		[JsonProperty("totalPages")]
		public int TotalPages { get; set; }
	}
}
