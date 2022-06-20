using TickerSubscriptionDemo.Application.Subscriptions.Models;

namespace TickerSubscriptionDemo.Application.Subscriptions.Transformers;

/// <summary>
/// Transformer for subscription requests from an expected model type.
/// </summary>
/// <typeparam name="TRequest">The request model type.</typeparam>
public interface ISubscriptionRequestTransformer<in TRequest>
{
    /// <summary>
    /// Creates subscription requests from a given collection of request models, as per a specific subscription type.
    /// </summary>
    /// <param name="models">The request models.</param>
    /// <param name="type">The subscription type.</param>
    /// <returns>The collection of corresponding subscription requests.</returns>
    IReadOnlyList<SubscriptionRequest> FromModels(IEnumerable<TRequest> models, SubscriptionType type);
}