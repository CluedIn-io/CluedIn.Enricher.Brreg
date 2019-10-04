using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class IndustryCode1
	{
		[DeserializeAs(Name = "kode")]
		public string Code { get; set; }

		[DeserializeAs(Name = "beskrivelse")]
		public string Description { get; set; }
	}
}