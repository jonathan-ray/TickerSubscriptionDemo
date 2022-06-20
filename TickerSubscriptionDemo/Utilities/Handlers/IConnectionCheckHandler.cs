namespace TickerSubscriptionDemo.Utilities.Handlers;

/// <summary>
/// Handler for checking whether a connection is still active.
/// </summary>
public interface IConnectionCheckHandler : IDisposable
{
    /// <summary>
    /// Starts the connection checking process.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    void Start(CancellationToken cancellationToken);
}