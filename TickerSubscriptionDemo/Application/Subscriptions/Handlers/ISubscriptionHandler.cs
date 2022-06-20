using Newtonsoft.Json.Linq;

namespace TickerSubscriptionDemo.Application.Subscriptions.Handlers;

/// <summary>
/// Handler for a subscription service.
/// </summary>
public interface ISubscriptionHandler
{
    /// <summary>
    /// What to do on receiving a subscription response.
    /// </summary>
    /// <param name="response">The response.</param>
    Task OnResponseReceived(JToken response);
}