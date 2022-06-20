namespace TickerSubscriptionDemo.Services.Contracts;

/// <summary>
/// A service that requires a connection stage to use.
/// </summary>
public interface IConnectableService : IDisposable
{
    /// <summary>
    /// Connect to the service.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Connect(CancellationToken cancellationToken);
}