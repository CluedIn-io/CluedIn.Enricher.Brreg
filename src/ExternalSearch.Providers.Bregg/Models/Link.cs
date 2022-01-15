using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Link
	{
		[DeserializeAs(Name = "rel")]
		public string Rel { get; set; }

		[DeserializeAs(Name = "href")]
		public string Href { get; set; }
	}
}