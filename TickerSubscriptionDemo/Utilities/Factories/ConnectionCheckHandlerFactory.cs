using Microsoft.Extensions.Options;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Settings;
using TickerSubscriptionDemo.Utilities.Handlers;

namespace TickerSubscriptionDemo.Utilities.Factories;

public class ConnectionCheckHandlerFactory : IConnectionCheckHandlerFactory
{
    private readonly ConnectionCheckSettings settings;

    public ConnectionCheckHandlerFactory(IOptions<ConnectionCheckSettings> settings)
    {
        this.settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public IConnectionCheckHandler Create(ITestConnectionService testService, IConnectableService connectionService)
    {
        return new ConnectionCheckHandler(
            testService,
            connectionService,
            this.settings);
    }
}