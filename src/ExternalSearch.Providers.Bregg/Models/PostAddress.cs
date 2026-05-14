using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class PostAddress
	{
		[JsonPropertyName("adresse")]
		public List<string> Address { get; set; }

		[JsonPropertyName("postnummer")]
		public string PostalCode { get; set; }

		[JsonPropertyName("poststed")]
		public string PostalArea { get; set; }

		[JsonPropertyName("kommunenummer")]
		public string MunicipalityNumber { get; set; }

		[JsonPropertyName("kommune")]
		public string Municipality { get; set; }

		[JsonPropertyName("landkode")]
		public string CountryCode { get; set; }

		[JsonPropertyName("land")]
		public string Country { get; set; }
	}
}
