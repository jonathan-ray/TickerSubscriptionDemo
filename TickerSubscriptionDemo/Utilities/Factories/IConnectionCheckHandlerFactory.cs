using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Utilities.Handlers;

namespace TickerSubscriptionDemo.Utilities.Factories;

/// <summary>
/// Factory for creating a connection check handler.
/// </summary>
public interface IConnectionCheckHandlerFactory
{
    /// <summary>
    /// Creates a connection check handler instance.
    /// </summary>
    /// <param name="testService">The service used to test whether a connection is active.</param>
    /// <param name="connectionService">The service used to reconnect, if necessary.</param>
    /// <returns>The connection check handler instance.</returns>
    IConnectionCheckHandler Create(
        ITestConnectionService testService,
        IConnectableService connectionService);
}