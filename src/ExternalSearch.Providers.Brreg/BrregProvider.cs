using CluedIn.Core;
using CluedIn.Core.Crawling;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using ExecutionContext = CluedIn.Core.ExecutionContext;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public partial class BrregProvider : ProviderBase, IExtendedProviderMetadata, IExternalSearchProviderProvider
{
    public BrregProvider(ApplicationContext appContext) : base(
        appContext, GetMetaData())
    {
        ExternalSearchProvider = appContext.Container.ResolveAll<IExternalSearchProvider>()
            .Single(n => n.Id == BrregConstants.ProviderId);
    }


    public override bool ScheduleCrawlJobs => false;

    public string Icon { get; } = BrregConstants.Icon;
    public string Domain { get; } = BrregConstants.Domain;
    public string About { get; } = BrregConstants.About;
    public AuthMethods AuthMethods { get; } = BrregConstants.AuthMethods;
    public IEnumerable<Control> Properties { get; } = Array.Empty<Control>();
    public Guide Guide { get; } = new();
    public new IntegrationType Type { get; } = BrregConstants.IntegrationType;
    public IExternalSearchProvider ExternalSearchProvider { get; }

    private static IProviderMetadata GetMetaData()
    {
        return new ProviderMetadata
        {
            Id = BrregConstants.ProviderId,
            Name = BrregConstants.ProviderName,
            ComponentName = BrregConstants.ComponentName,
            AuthTypes = new List<string>(),
            SupportsConfiguration = true,
            SupportsAutomaticWebhookCreation = false,
            SupportsWebHooks = false,
            Type = "Enricher"
        };
    }

    public override async Task<CrawlJobData> GetCrawlJobData(ProviderUpdateContext context,
        IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var result = new BrregJobData(configuration);

        return await Task.FromResult(result);
    }

    public override async Task<IDictionary<string, object?>> GetHelperConfiguration(ProviderUpdateContext context,
        CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        if (jobData is BrregJobData result) return await Task.FromResult(result.ToDictionary());

        throw new InvalidOperationException(
            $"Unexpected data type for {nameof(BrregJobData)}, {jobData.GetType()}");
    }

    public override Task<IDictionary<string, object?>> GetHelperConfiguration(ProviderUpdateContext context,
        CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId, string folderId)
    {
        return GetHelperConfiguration(context, jobData, organizationId, userId, providerDefinitionId);
    }

    public override Task<AccountInformation> GetAccountInformation(ExecutionContext context, CrawlJobData jobData,
        Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        return Task.FromResult(new AccountInformation(providerDefinitionId.ToString(),
            providerDefinitionId.ToString()));
    }
}