using System.Collections.Generic;
using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Brreg.Models
{
	public class RootBrregOrganization
	{
		[DeserializeAs(Name = "data")]
		public List<BrregOrganization> Data { get; set; }

		[DeserializeAs(Name = "links")]
		public List<Link> Links { get; set; }

		[DeserializeAs(Name = "page")]
		public Page Page { get; set; }
	}
}