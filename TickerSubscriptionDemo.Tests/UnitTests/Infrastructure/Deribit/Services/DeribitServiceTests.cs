using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Infrastructure.Deribit.Proxies.Models;
using TickerSubscriptionDemo.Infrastructure.Deribit.Services;
using TickerSubscriptionDemo.Services;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Utilities.Factories;
using TickerSubscriptionDemo.Utilities.Handlers;

namespace TickerSubscriptionDemo.Tests.UnitTests.Infrastructure.Deribit.Services;

[Trait("Category", "UnitTests")]
public class DeribitServiceTests
{
    private readonly Mock<IDeribitRpcClientService> clientServiceMock;
    private readonly Mock<IJsonRpcService> rpcServiceMock;
    private readonly Mock<IConnectionCheckHandler> connectionCheckMock;

    private readonly IDataService serviceUnderTest;

    public DeribitServiceTests()
    {
        this.clientServiceMock = new Mock<IDeribitRpcClientService>();
        this.rpcServiceMock = new Mock<IJsonRpcService>();
        this.connectionCheckMock = new Mock<IConnectionCheckHandler>();
        var connectionCheckFactoryMock = new Mock<IConnectionCheckHandlerFactory>();
        connectionCheckFactoryMock
            .Setup(m => m.Create(It.IsAny<ITestConnectionService>(), It.IsAny<IConnectableService>()))
            .Returns(this.connectionCheckMock.Object);

        this.serviceUnderTest = new DeribitService(
            this.clientServiceMock.Object,
            this.rpcServiceMock.Object,
            connectionCheckFactoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullDeribitClientService_ShouldThrow()
    {
        var construction = () => new DeribitService(
            deribitClientService: null!,
            Mock.Of<IJsonRpcService>(),
            Mock.Of<IConnectionCheckHandlerFactory>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("deribitClientService");
    }

    [Fact]
    public void Construction_WithNullJsonRpcService_ShouldThrow()
    {
        var construction = () => new DeribitService(
            Mock.Of<IDeribitRpcClientService>(),
            jsonRpcService: null!,
            Mock.Of<IConnectionCheckHandlerFactory>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("jsonRpcService");
    }

    [Fact]
    public void Construction_WithNullConnectionCheckServiceFactory_ShouldThrow()
    {
        var construction = () => new DeribitService(
            Mock.Of<IDeribitRpcClientService>(),
            Mock.Of<IJsonRpcService>(),
            connectionCheckServiceFactory: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("connectionCheckServiceFactory");
    }

    [Fact]
    public async Task Connect_ShouldAttemptToConnect()
    {
        await this.serviceUnderTest.Connect(CancellationToken.None);

        this.rpcServiceMock.Verify(m =>
            m.Connect(this.clientServiceMock.Object, It.IsAny<CancellationToken>()),
            Times.Once);

        this.connectionCheckMock.Verify(m =>
            m.Start(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencies_WithData_ShouldReturnSuccessfully()
    {
        var data = new[]
        {
            new CurrencyDataModel { currency = "ABC" },
            new CurrencyDataModel { currency = "XYZ" }
        };

        this.clientServiceMock
            .Setup(m => m.GetCurrencies(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        var actualCurrencies = await this.serviceUnderTest.GetCurrencies(CancellationToken.None);

        actualCurrencies.Should().HaveCount(data.Length);
        actualCurrencies[0].Id.Should().Be(data[0].currency);
        actualCurrencies[1].Id.Should().Be(data[1].currency);
    }

    [Fact]
    public async Task GetCurrencies_WithoutData_ShouldReturnSuccessfully()
    {
        this.clientServiceMock
            .Setup(m => m.GetCurrencies(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<CurrencyDataModel>());

        var actualCurrencies = await this.serviceUnderTest.GetCurrencies(CancellationToken.None);

        actualCurrencies.Should().BeEmpty();
    }

    [Fact]
    public async Task GetInstrumentsForCurrency_WithNullCurrency_ShouldThrow()
    {
        var exceptionAssertion = await this.serviceUnderTest
            .Awaiting(s => s.GetInstrumentsForCurrency(currency: null!, CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>();

        exceptionAssertion.And.ParamName.Should().Be("currency");
    }

    [Fact]
    public async Task GetInstrumentsForCurrency_WithData_ShouldReturnSuccessfully()
    {
        const string currencyId = "TWO";

        var data = new[]
        {
            new InstrumentDataModel { instrument_name = "TWO-Instrument-001" },
            new InstrumentDataModel { instrument_name = "TWO-Instrument-002" }
        };

        this.clientServiceMock
            .Setup(m => m.GetInstrumentsForCurrency(currencyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        var actualInstruments = await this.serviceUnderTest.GetInstrumentsForCurrency(
            new Currency(currencyId),
            CancellationToken.None);

        actualInstruments.Should().HaveCount(data.Length);
        actualInstruments[0].Name.Should().Be(data[0].instrument_name);
        actualInstruments[1].Name.Should().Be(data[1].instrument_name);
    }

    [Fact]
    public async Task GetInstrumentsForCurrency_WithoutData_ShouldReturnSuccessfully()
    {
        const string currencyId = "TWO";

        this.clientServiceMock
            .Setup(m => m.GetInstrumentsForCurrency(currencyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<InstrumentDataModel>());

        var actualInstruments = await this.serviceUnderTest.GetInstrumentsForCurrency(
            new Currency(currencyId),
            CancellationToken.None);

        actualInstruments.Should().BeEmpty();
    }

    [Fact]
    public async Task Subscribe_WithNullRequests_ShouldThrow()
    {
        var exceptionAssertion = await this.serviceUnderTest
            .Awaiting(s => s.Subscribe(
                requests: null!, 
                Mock.Of<ISubscriptionHandler>(),
                CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>();

        exceptionAssertion.And.ParamName.Should().Be("requests");
    }

    [Fact]
    public async Task Subscribe_WithEmptyRequests_ShouldThrow()
    {
        var exceptionAssertion = await this.serviceUnderTest
            .Awaiting(s => s.Subscribe(
                requests: Array.Empty<SubscriptionRequest>(), 
                Mock.Of<ISubscriptionHandler>(),
                CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();

        exceptionAssertion.And.ParamName.Should().Be("requests");
    }

    [Fact]
    public async Task Subscribe_WithNullSubscriptionHandler_ShouldThrow()
    {
        var exceptionAssertion = await this.serviceUnderTest
            .Awaiting(s => s.Subscribe(
                new[] { new SubscriptionRequest(SubscriptionType.Ticker, "ABC-I-001", 100) },
                subscriptionHandler: null!,
                CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();

        exceptionAssertion.And.ParamName.Should().Be("subscriptionHandler");
    }

    [Fact]
    public async Task Subscribe_WithValidRequests_ShouldSubscribe()
    {
        var requests = new[] { new SubscriptionRequest(SubscriptionType.Ticker, "ABC-I-001", 100) };

        await this.serviceUnderTest.Subscribe(
            requests,
            Mock.Of<ISubscriptionHandler>(),
            CancellationToken.None);

        this.clientServiceMock.Verify(m =>
            m.Subscribe(requests, It.IsAny<Func<JToken, Task>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UnsubscribeAll_ShouldUnsubscribeSuccessfully()
    {
        await this.serviceUnderTest.UnsubscribeAll(CancellationToken.None);

        this.clientServiceMock.Verify(m =>
            m.UnsubscribeAll(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}