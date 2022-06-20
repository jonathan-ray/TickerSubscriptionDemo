using Newtonsoft.Json.Linq;

namespace TickerSubscriptionDemo.Application.Subscriptions.Transformers;

/// <summary>
/// Transformer for subscription responses to an expected model type.
/// </summary>
/// <typeparam name="TSubscriptionResponse">The response type.</typeparam>
public interface ISubscriptionResponseTransformer<out TSubscriptionResponse>
{
    /// <summary>
    /// Creates a subscription response model from the received JSON token.
    /// </summary>
    /// <param name="response">The response as a JSON token.</param>
    /// <returns>The subscription response model.</returns>
    TSubscriptionResponse FromJson(JToken response);
}