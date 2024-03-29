﻿using RestSharp.Deserializers;

namespace CluedIn.ExternalSearch.Providers.Bregg.Models
{
	public class Page
	{

		[DeserializeAs(Name = "size")]
		public int Size { get; set; }

		[DeserializeAs(Name = "page")]
		public int PageCount { get; set; }
	}
}