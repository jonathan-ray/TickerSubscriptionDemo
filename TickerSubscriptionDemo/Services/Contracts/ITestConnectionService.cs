namespace TickerSubscriptionDemo.Services.Contracts;

/// <summary>
/// A service that has the ability to test whether the connection is active.
/// </summary>
public interface ITestConnectionService
{
    /// <summary>
    /// Runs the connection test.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task RunConnectionTest(CancellationToken cancellationToken);
}