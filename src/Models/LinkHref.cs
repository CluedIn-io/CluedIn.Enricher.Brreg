using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class LinkHref
	{
		[DeserializeAs(Name = "href")]
		public string Href { get; set; }
	}
}