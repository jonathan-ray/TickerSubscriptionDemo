using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Infrastructure.Deribit.Proxies.Models;
using TickerSubscriptionDemo.Services.Contracts;

namespace TickerSubscriptionDemo.Infrastructure.Deribit.Services;

/// <summary>
/// The Deribit JSON-RPC client service.
/// </summary>
/// <remarks>A thin layer to abstract away dynamic client proxy internals.</remarks>
public interface IDeribitRpcClientService : ITestConnectionService, IRpcClientService, IDisposable
{
    /// <summary>
    /// Gets all currencies.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of all currencies.</returns>
    Task<CurrencyDataModel[]> GetCurrencies(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all instruments for a specific currency.
    /// </summary>
    /// <param name="currencyId">The currency's ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of relevant instruments.</returns>
    Task<InstrumentDataModel[]> GetInstrumentsForCurrency(string currencyId, CancellationToken cancellationToken);

    /// <summary>
    /// Subscribes to a collection of requests.
    /// </summary>
    /// <param name="requests">The subscription requests.</param>
    /// <param name="onResponseReceived">Function to invoke on response received.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task Subscribe(SubscriptionRequest[] requests, Func<JToken, Task> onResponseReceived, CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribes from all active requests.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UnsubscribeAll(CancellationToken cancellationToken);
}