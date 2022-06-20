using TickerSubscriptionDemo.Services.Contracts;

namespace TickerSubscriptionDemo.Services;

/// <summary>
/// Service for connection to a JSON-RPC stream.
/// </summary>
public interface IJsonRpcService : IDisposable
{
    /// <summary>
    /// Connects to the service.
    /// </summary>
    /// <param name="rpcClientService">The client that will interact with this service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Connect(IRpcClientService rpcClientService, CancellationToken cancellationToken);
}