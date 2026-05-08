using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class RootBrregOrganization
	{
		[JsonPropertyName("_embedded")]
		public Unit Embedded { get; set; }

		[JsonPropertyName("_links")]
		public List<Link> Links { get; set; }

		[JsonPropertyName("page")]
		public Page Page { get; set; }
	}
}
