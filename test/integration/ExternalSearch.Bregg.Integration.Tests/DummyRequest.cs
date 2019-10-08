using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Parts;
using CluedIn.ExternalSearch;

namespace ExternalSearch.Bregg.Integration.Tests
{
	public class DummyRequest : IExternalSearchRequest
	{
		public IEntityMetadata EntityMetaData
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public object CustomQueryInput
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool? NoRecursion
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public List<Guid> ProviderIds
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public IExternalSearchQueryParameters QueryParameters
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public List<IExternalSearchQuery> Queries
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool IsFinished
		{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool AllQueriesHasExecuted => throw new NotImplementedException();
	}
}
