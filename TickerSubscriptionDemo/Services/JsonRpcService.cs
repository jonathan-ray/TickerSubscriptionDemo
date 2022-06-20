using System.Net.WebSockets;
using Microsoft.Extensions.Options;
using StreamJsonRpc;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Settings;

namespace TickerSubscriptionDemo.Services;

public class JsonRpcService : IJsonRpcService
{
    private readonly WebSocketSettings settings;

    private ClientWebSocket? webSocket;
    private JsonRpc? rpcConnection;

    public JsonRpcService(IOptions<WebSocketSettings> settings)
    {
        this.settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task Connect(IRpcClientService rpcClientService, CancellationToken cancellationToken)
    {
        if (rpcClientService is null)
        {
            throw new ArgumentNullException(nameof(rpcClientService));
        }

        this.webSocket = new ClientWebSocket();
        await this.webSocket.ConnectAsync(new Uri(this.settings.WebSocketUri), cancellationToken);

        var messageHandler = new WebSocketMessageHandler(this.webSocket);
        this.rpcConnection = new JsonRpc(messageHandler);

        rpcClientService.AttachToConnection(this.rpcConnection);

        this.rpcConnection.StartListening();
    }

    public void Dispose()
    {
        this.rpcConnection?.Dispose();
        this.webSocket?.Dispose();
    }
}