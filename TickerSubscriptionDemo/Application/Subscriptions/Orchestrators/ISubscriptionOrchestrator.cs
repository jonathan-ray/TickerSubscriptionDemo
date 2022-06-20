namespace TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;

/// <summary>
/// Orchestrator for subscriptions.
/// </summary>
public interface ISubscriptionOrchestrator
{
    /// <summary>
    /// Starts orchestration of subscriptions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Start(CancellationToken cancellationToken);

    /// <summary>
    /// Stops orchestration of subscriptions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Stop(CancellationToken cancellationToken);
}