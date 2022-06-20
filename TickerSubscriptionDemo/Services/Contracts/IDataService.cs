using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Domain.Models;

namespace TickerSubscriptionDemo.Services.Contracts;

/// <summary>
/// Service to retrieve the required data.
/// </summary>
public interface IDataService : IConnectableService
{
    /// <summary>
    /// Gets all currencies from the market data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of currencies.</returns>
    Task<IReadOnlyList<Currency>> GetCurrencies(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all instruments for a given currency.
    /// </summary>
    /// <param name="currency">The currency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of instruments.</returns>
    Task<IReadOnlyList<Instrument>> GetInstrumentsForCurrency(Currency currency, CancellationToken cancellationToken);

    /// <summary>
    /// Subscribes to a collection of requests.
    /// </summary>
    /// <param name="requests">The subscription requests.</param>
    /// <param name="subscriptionHandler">The handler for subscription actions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Subscribe(IReadOnlyList<SubscriptionRequest> requests, ISubscriptionHandler subscriptionHandler, CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribes to all currently active subscriptions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UnsubscribeAll(CancellationToken cancellationToken);
}