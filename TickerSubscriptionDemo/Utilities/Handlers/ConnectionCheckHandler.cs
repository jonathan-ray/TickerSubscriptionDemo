using System.Net.WebSockets;
using StreamJsonRpc;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Settings;

namespace TickerSubscriptionDemo.Utilities.Handlers;

public class ConnectionCheckHandler : IConnectionCheckHandler
{
    private readonly ITestConnectionService testService;
    private readonly IConnectableService connectionService;
    private readonly ConnectionCheckSettings settings;
    private Timer? checkTimer;
    private readonly SemaphoreSlim connectionCheckSemaphore = new(1, 1);

    public ConnectionCheckHandler(
        ITestConnectionService testService,
        IConnectableService connectionService,
        ConnectionCheckSettings settings)
    {
        this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
        this.connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public void Start(CancellationToken cancellationToken)
    {
        this.checkTimer = new Timer(
            _ => this.CheckConnection(cancellationToken),
            null,
            TimeSpan.Zero,
            this.settings.CheckIntervalPeriod);
    }

    private async void CheckConnection(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            this.checkTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            return;
        }

        try
        {
            await this.connectionCheckSemaphore.WaitAsync(cancellationToken);
            await this.testService.RunConnectionTest(cancellationToken);
        }
        catch (ConnectionLostException)
        {
            await this.connectionService.Connect(cancellationToken);
        }
        catch (WebSocketException exception)
            when (exception.WebSocketErrorCode.Equals(WebSocketError.ConnectionClosedPrematurely))
        {
            await this.connectionService.Connect(cancellationToken);
        }
        finally
        {
            this.connectionCheckSemaphore.Release();
        }
    }

    public void Dispose()
    {
        this.checkTimer?.Dispose();
        this.connectionCheckSemaphore.Dispose();
    }
}