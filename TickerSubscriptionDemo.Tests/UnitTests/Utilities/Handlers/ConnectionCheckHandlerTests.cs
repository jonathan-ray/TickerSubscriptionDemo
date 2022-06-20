using System.Net.WebSockets;
using StreamJsonRpc;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Settings;
using TickerSubscriptionDemo.Utilities.Handlers;

namespace TickerSubscriptionDemo.Tests.UnitTests.Utilities.Handlers;

[Trait("Category", "UnitTests")]
public class ConnectionCheckHandlerTests
{
    private static readonly ConnectionCheckSettings DefaultSettings = new() { CheckIntervalPeriod = TimeSpan.FromMilliseconds(200) };

    private readonly Mock<ITestConnectionService> testConnectionMock;
    private readonly Mock<IConnectableService> connectingServiceMock;

    private readonly IConnectionCheckHandler handlerUnderTest;

    public ConnectionCheckHandlerTests()
    {
        this.testConnectionMock = new Mock<ITestConnectionService>();
        this.connectingServiceMock = new Mock<IConnectableService>();

        this.handlerUnderTest = new ConnectionCheckHandler(
            this.testConnectionMock.Object,
            this.connectingServiceMock.Object,
            DefaultSettings);
    }

    [Fact]
    public void Construction_WithNullTestService_ShouldThrow()
    {
        var construction = () => new ConnectionCheckHandler(
            testService: null!,
            Mock.Of<IConnectableService>(),
            DefaultSettings);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("testService");
    }

    [Fact]
    public void Construction_WithNullConnectionService_ShouldThrow()
    {
        var construction = () => new ConnectionCheckHandler(
            Mock.Of<ITestConnectionService>(),
            connectionService: null!,
            DefaultSettings);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("connectionService");
    }

    [Fact]
    public void Construction_WithNullSettings_ShouldThrow()
    {
        var construction = () => new ConnectionCheckHandler(
            Mock.Of<ITestConnectionService>(),
            Mock.Of<IConnectableService>(),
            settings: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("settings");
    }

    [Fact]
    public async Task Create_WithConnectionOk_ShouldContinue()
    {
        var tokenSource = new CancellationTokenSource();
        this.handlerUnderTest.Start(tokenSource.Token);

        // 500 for calls every 200 => expect 3 checks (0, ~200, ~400).
        await Task.Delay(500);

        this.testConnectionMock.Verify(m =>
            m.RunConnectionTest(It.IsAny<CancellationToken>()),
            Times.Exactly(3));

        this.connectingServiceMock.Verify(m =>
                m.Connect(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_WithConnectionLostException_ShouldReconnect()
    {
        this.testConnectionMock
            .Setup(m => m.RunConnectionTest(It.IsAny<CancellationToken>()))
            .Throws<ConnectionLostException>();

        var tokenSource = new CancellationTokenSource();
        this.handlerUnderTest.Start(tokenSource.Token);

        await Task.Delay(100);

        this.testConnectionMock.Verify(m =>
                m.RunConnectionTest(It.IsAny<CancellationToken>()),
            Times.Once);

        this.connectingServiceMock.Verify(m =>
                m.Connect(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_WithKnownWebSocketException_ShouldReconnect()
    {
        this.testConnectionMock
            .Setup(m => m.RunConnectionTest(It.IsAny<CancellationToken>()))
            .Throws(new WebSocketException(WebSocketError.ConnectionClosedPrematurely));

        var tokenSource = new CancellationTokenSource();
        this.handlerUnderTest.Start(tokenSource.Token);

        await Task.Delay(100);

        this.testConnectionMock.Verify(m =>
                m.RunConnectionTest(It.IsAny<CancellationToken>()),
            Times.Once);

        this.connectingServiceMock.Verify(m =>
                m.Connect(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_WithCancelledToken_ShouldDoNothing()
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        this.handlerUnderTest.Start(tokenSource.Token);

        await Task.Delay(100);

        this.testConnectionMock.Verify(m =>
                m.RunConnectionTest(It.IsAny<CancellationToken>()),
            Times.Never);

        this.connectingServiceMock.Verify(m =>
                m.Connect(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}