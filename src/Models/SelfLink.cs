using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class SelfLink
	{
		[DeserializeAs(Name = "self")]
		public LinkHref Self { get; set; }
	}
}