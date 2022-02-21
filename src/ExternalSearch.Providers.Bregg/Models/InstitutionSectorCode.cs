using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class InstitutionSectorCode
	{
		[DeserializeAs(Name = "kode")]
		public string Code { get; set; }

		[DeserializeAs(Name = "beskrivelse")]
		public string Description { get; set; }
	}
}