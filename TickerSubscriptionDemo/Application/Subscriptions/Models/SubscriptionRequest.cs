namespace TickerSubscriptionDemo.Application.Subscriptions.Models;

/// <summary>
/// A subscription request.
/// </summary>
/// <param name="Type">Type of subscription.</param>
/// <param name="Name">Name of subscription.</param>
/// <param name="IntervalMs">Interval of subscription requests in milliseconds.</param>
public record SubscriptionRequest(SubscriptionType Type, string Name, long IntervalMs)
{
    /// <summary>
    /// The Channel name representation of the request.
    /// </summary>
    public string ChannelName => $"{this.Type.ToString().ToLowerInvariant()}.{this.Name}.{this.IntervalMs}ms";
}