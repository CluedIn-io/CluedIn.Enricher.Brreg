using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class RootBrregOrganization
	{
		[JsonProperty("_embedded")]
		public Unit Embedded { get; set; }

		[JsonProperty("_links")]
		public Dictionary<string, LinkHref> Links { get; set; }

		[JsonProperty("page")]
		public Page Page { get; set; }
	}
}
