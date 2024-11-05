using System.Collections.Generic;
using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class RootBrregOrganization
	{
		[DeserializeAs(Name = "embedded")]
		public Unit Embedded { get; set; }

		[DeserializeAs(Name = "links")]
		public List<Link> Links { get; set; }

		[DeserializeAs(Name = "page")]
		public Page Page { get; set; }
	}
}