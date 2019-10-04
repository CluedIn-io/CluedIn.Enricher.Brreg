using System.Collections.Generic;
using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class PostAddress
	{
		[DeserializeAs(Name = "adresse")]
		public List<string> Address { get; set; } 

		[DeserializeAs(Name = "postnummer")]
		public string PostalCode { get; set; }

		[DeserializeAs(Name = "poststed")]
		public string PostalArea { get; set; }

		[DeserializeAs(Name = "kommunenummer")]
		public string MunicipalityNumber { get; set; }

		[DeserializeAs(Name = "kommune")]
		public string Municipality { get; set; }

		[DeserializeAs(Name = "landkode")]
		public string CountryCode { get; set; }

		[DeserializeAs(Name = "land")]
		public string Country { get; set; }
	}
}