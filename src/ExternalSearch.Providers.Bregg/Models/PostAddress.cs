using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class PostAddress
	{
		[JsonProperty("adresse")]
		public List<string> Address { get; set; }

		[JsonProperty("postnummer")]
		public string PostalCode { get; set; }

		[JsonProperty("poststed")]
		public string PostalArea { get; set; }

		[JsonProperty("kommunenummer")]
		public string MunicipalityNumber { get; set; }

		[JsonProperty("kommune")]
		public string Municipality { get; set; }

		[JsonProperty("landkode")]
		public string CountryCode { get; set; }

		[JsonProperty("land")]
		public string Country { get; set; }
	}
}
