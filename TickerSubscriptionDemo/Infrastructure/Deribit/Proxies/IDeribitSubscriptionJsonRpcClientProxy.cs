using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace TickerSubscriptionDemo.Infrastructure.Deribit.Proxies;

/// <summary>
/// The dynamic client proxy for Deribit methods.
/// </summary>
/// <remarks>Contains all Subscription-related calls.</remarks>
public partial interface IDeribitJsonRpcClientProxy
{
    /// <summary>
    /// Event handler for subscription responses.
    /// </summary>
    event EventHandler<JToken> subscription;

    /// <summary>
    /// Subscribes to a set of channels.
    /// </summary>
    /// <param name="channels">The channels.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    [JsonRpcMethod("public/subscribe")]
    Task Subscribe(string[] channels, CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribes to all channels.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    [JsonRpcMethod("public/unsubscribe_all")]
    Task UnsubscribeAll(CancellationToken cancellationToken);
}