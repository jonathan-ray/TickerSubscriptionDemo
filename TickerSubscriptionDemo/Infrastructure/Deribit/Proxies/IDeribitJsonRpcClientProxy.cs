using StreamJsonRpc;

namespace TickerSubscriptionDemo.Infrastructure.Deribit.Proxies;

/// <summary>
/// The dynamic client proxy for Deribit methods.
/// </summary>
public partial interface IDeribitJsonRpcClientProxy : IDisposable
{
    /// <summary>
    /// Method for testing the connection.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    [JsonRpcMethod("public/test")]
    Task TestConnection(CancellationToken cancellationToken);
}