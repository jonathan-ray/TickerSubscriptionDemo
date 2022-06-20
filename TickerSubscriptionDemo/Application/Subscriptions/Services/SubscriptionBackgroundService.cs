using TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;

namespace TickerSubscriptionDemo.Application.Subscriptions.Services;

public class SubscriptionBackgroundService : IHostedService
{
    private readonly ISubscriptionOrchestrator subscriptionOrchestrator;

    public SubscriptionBackgroundService(ISubscriptionOrchestrator subscriptionOrchestrator)
    {
        this.subscriptionOrchestrator = subscriptionOrchestrator ?? throw new ArgumentNullException(nameof(subscriptionOrchestrator));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return this.subscriptionOrchestrator.Start(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return this.subscriptionOrchestrator.Stop(cancellationToken);
    }
}