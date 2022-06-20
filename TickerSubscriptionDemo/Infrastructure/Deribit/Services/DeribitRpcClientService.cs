using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Infrastructure.Deribit.Proxies;
using TickerSubscriptionDemo.Infrastructure.Deribit.Proxies.Models;
using TickerSubscriptionDemo.Services.Exceptions;

namespace TickerSubscriptionDemo.Infrastructure.Deribit.Services;

public class DeribitRpcClientService : IDeribitRpcClientService
{
    private IDeribitJsonRpcClientProxy? dynamicClientProxy;

    public void AttachToConnection(JsonRpc rpcConnection)
    {
        this.dynamicClientProxy = rpcConnection.Attach<IDeribitJsonRpcClientProxy>(new JsonRpcProxyOptions
        {
            ServerRequiresNamedArguments = true
        });
    }

    public Task RunConnectionTest(CancellationToken cancellationToken)
    {
        VerifyIsAttached(this.dynamicClientProxy);
        return this.dynamicClientProxy.TestConnection(cancellationToken);
    }

    public Task<CurrencyDataModel[]> GetCurrencies(CancellationToken cancellationToken)
    {
        VerifyIsAttached(this.dynamicClientProxy);
        return this.dynamicClientProxy.GetCurrencies(cancellationToken);
    }

    public Task<InstrumentDataModel[]> GetInstrumentsForCurrency(string currencyId, CancellationToken cancellationToken)
    {
        VerifyIsAttached(this.dynamicClientProxy);
        return this.dynamicClientProxy.GetInstrumentsPerCurrency(currencyId, cancellationToken);
    }

    public async Task Subscribe(SubscriptionRequest[] requests, Func<JToken, Task> onResponseReceived, CancellationToken cancellationToken)
    {
        VerifyIsAttached(this.dynamicClientProxy);
        this.dynamicClientProxy.subscription += (_, token) => onResponseReceived(token);

        await this.dynamicClientProxy.Subscribe(
            requests.Select(r => r.ChannelName).ToArray(),
            cancellationToken);
    }

    public Task UnsubscribeAll(CancellationToken cancellationToken)
    {
        VerifyIsAttached(this.dynamicClientProxy);
        return this.dynamicClientProxy.UnsubscribeAll(cancellationToken);
    }

    public void Dispose()
    {
        this.dynamicClientProxy?.Dispose();
    }

    private static void VerifyIsAttached([NotNull] IDeribitJsonRpcClientProxy? clientProxy)
    {
        if (clientProxy is null)
        {
            throw new UnattachedRpcClientServiceException();
        }
    }
}