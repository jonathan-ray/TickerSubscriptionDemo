using StreamJsonRpc;

namespace TickerSubscriptionDemo.Services.Contracts;

/// <summary>
/// Client service for interacting with a JSON-RPC service.
/// </summary>
public interface IRpcClientService
{
    /// <summary>
    /// Attaches its internal methods to the given JSON-RPC connection.
    /// </summary>
    /// <param name="rpcConnection">The JSON-RPC connection.</param>
    void AttachToConnection(JsonRpc rpcConnection);
}