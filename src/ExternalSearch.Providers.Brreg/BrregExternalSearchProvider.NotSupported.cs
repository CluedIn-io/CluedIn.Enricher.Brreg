using CluedIn.Core.Data;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;
using ExecutionContext = CluedIn.Core.ExecutionContext;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public partial class BrregExternalSearchProvider
{
    public IPreviewImage? GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result,
        IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
    {
        return null;
    }

    public IEnumerable<Control> Properties { get; } = Array.Empty<Control>();
    public Guide Guide { get; } = new();

    public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query,
        IExternalSearchQueryResult result, IExternalSearchRequest request)
    {
        // this must be never invoked
        return Enumerable.Empty<Clue>();
    }

    public override IPreviewImage? GetPrimaryEntityPreviewImage(
        ExecutionContext context,
        IExternalSearchQueryResult result,
        IExternalSearchRequest request)
    {
        return null;
    }

    public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context,
        IExternalSearchRequest request)
    {
        // this must be never invoked
        return Enumerable.Empty<IExternalSearchQuery>();
    }

    public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context,
        IExternalSearchQuery query)
    {
        // this must be never invoked
        return Enumerable.Empty<IExternalSearchQueryResult>();
    }
}