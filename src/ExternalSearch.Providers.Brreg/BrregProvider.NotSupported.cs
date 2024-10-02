using CluedIn.Core.Crawling;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using CluedIn.Providers.Models;
using ExecutionContext = CluedIn.Core.ExecutionContext;

namespace CluedIn.ExternalSearch.Providers.Brreg;

public partial class BrregProvider
{
    public override Task<bool> TestAuthentication(ProviderUpdateContext context,
        IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        return Task.FromResult(true);
    }

    public override Task<ExpectedStatistics> FetchUnSyncedEntityStatistics(ExecutionContext context,
        IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        throw new NotImplementedException();
    }

    public override string Schedule(DateTimeOffset relativeDateTime, bool webHooksEnabled)
    {
        return $"{relativeDateTime.Minute} 0/23 * * *";
    }

    public override Task<IEnumerable<WebHookSignature>> CreateWebHook(ExecutionContext context, CrawlJobData jobData,
        IWebhookDefinition webhookDefinition, IDictionary<string, object> config)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<WebhookDefinition>> GetWebHooks(ExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteWebHook(ExecutionContext context, CrawlJobData jobData,
        IWebhookDefinition webhookDefinition)
    {
        throw new NotImplementedException();
    }

    public override Task<CrawlLimit> GetRemainingApiAllowance(ExecutionContext context, CrawlJobData jobData,
        Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
        if (jobData == null) throw new ArgumentNullException(nameof(jobData));
        return Task.FromResult(new CrawlLimit(-1, TimeSpan.Zero));
    }

    public override IEnumerable<string> WebhookManagementEndpoints(IEnumerable<string> ids)
    {
        throw new NotImplementedException();
    }
}