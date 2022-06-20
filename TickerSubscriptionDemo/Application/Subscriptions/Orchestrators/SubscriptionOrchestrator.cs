using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Services.Contracts;

namespace TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;

public class SubscriptionOrchestrator : ISubscriptionOrchestrator
{
    private readonly IDataService dataService;
    private readonly ISubscriptionHandler subscriptionHandler;
    private readonly ISubscriptionRequestTransformer<Instrument> requestTransformer;

    public SubscriptionOrchestrator(
        IDataService dataService, 
        ISubscriptionHandler subscriptionHandler,
        ISubscriptionRequestTransformer<Instrument> requestTransformer)
    {
        this.dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        this.subscriptionHandler = subscriptionHandler ?? throw new ArgumentNullException(nameof(subscriptionHandler));
        this.requestTransformer = requestTransformer ?? throw new ArgumentNullException(nameof(requestTransformer));
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        await this.ConnectToService(cancellationToken);
        var instruments = await this.GetInstruments(cancellationToken);
        await this.SubscribeToInstruments(instruments, cancellationToken);
    }

    public async Task Stop(CancellationToken cancellationToken)
    {
        await this.dataService.UnsubscribeAll(cancellationToken);
    }

    private Task ConnectToService(CancellationToken cancellationToken)
    {
        return this.dataService.Connect(cancellationToken);
    }

    private async Task<IEnumerable<Instrument>> GetInstruments(CancellationToken cancellationToken)
    {
        var currencies = await this.dataService.GetCurrencies(cancellationToken);
        var getInstrumentTasks = currencies.Select(currency => this.dataService.GetInstrumentsForCurrency(currency, cancellationToken));
        var instrumentsPerCurrencies = await Task.WhenAll(getInstrumentTasks);
        return instrumentsPerCurrencies.SelectMany(instruments => instruments);
    }

    private async Task SubscribeToInstruments(IEnumerable<Instrument> instruments, CancellationToken cancellationToken)
    {
        var requests = this.requestTransformer.FromModels(instruments, SubscriptionType.Ticker);

        await this.dataService.Subscribe(
            requests,
            this.subscriptionHandler,
            cancellationToken);
    }
}