using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Services;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Utilities.Factories;
using TickerSubscriptionDemo.Utilities.Handlers;

namespace TickerSubscriptionDemo.Infrastructure.Deribit.Services;

public class DeribitService : IDataService
{
    private readonly IDeribitRpcClientService deribitClientService;
    private readonly IJsonRpcService jsonRpcService;
    private readonly IConnectionCheckHandler connectionCheckService;

    public DeribitService(
        IDeribitRpcClientService deribitClientService,
        IJsonRpcService jsonRpcService,
        IConnectionCheckHandlerFactory connectionCheckServiceFactory)
    {
        this.deribitClientService = deribitClientService ?? throw new ArgumentNullException(nameof(deribitClientService));
        this.jsonRpcService = jsonRpcService ?? throw new ArgumentNullException(nameof(jsonRpcService));

        if (connectionCheckServiceFactory is null)
        {
            throw new ArgumentNullException(nameof(connectionCheckServiceFactory));
        }
        this.connectionCheckService = connectionCheckServiceFactory.Create(this.deribitClientService, this);
    }

    public async Task Connect(CancellationToken cancellationToken)
    {
        await this.jsonRpcService.Connect(this.deribitClientService, cancellationToken);
        this.connectionCheckService.Start(cancellationToken);
    }

    public async Task<IReadOnlyList<Currency>> GetCurrencies(CancellationToken cancellationToken)
    {
        var currencyDataModels = await this.deribitClientService.GetCurrencies(cancellationToken);

        return currencyDataModels
            .Select(model => new Currency(model.currency))
            .ToList();
    }

    public async Task<IReadOnlyList<Instrument>> GetInstrumentsForCurrency(Currency currency, CancellationToken cancellationToken)
    {
        if (currency is null)
        {
            throw new ArgumentNullException(nameof(currency));
        }

        var inventoryDataModels = await this.deribitClientService.GetInstrumentsForCurrency(currency.Id, cancellationToken);

        return inventoryDataModels
            .Select(model => new Instrument(model.instrument_name))
            .ToList();
    }

    public async Task Subscribe(IReadOnlyList<SubscriptionRequest> requests, ISubscriptionHandler subscriptionHandler, CancellationToken cancellationToken)
    {
        if (requests is null)
        {
            throw new ArgumentNullException(nameof(requests));
        }

        if (requests.Count == 0)
        {
            throw new ArgumentException("There must be at least one request to subscribe to.", nameof(requests));
        }

        if (subscriptionHandler is null)
        {
            throw new ArgumentNullException(nameof(subscriptionHandler));
        }

        await this.deribitClientService.Subscribe(requests.ToArray(), subscriptionHandler.OnResponseReceived, cancellationToken);
    }

    public Task UnsubscribeAll(CancellationToken cancellationToken)
    {
        return this.deribitClientService.UnsubscribeAll(cancellationToken);
    }

    public void Dispose()
    {
        this.connectionCheckService.Dispose();
        this.deribitClientService.Dispose();
        this.jsonRpcService.Dispose();
    }
}