using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Services.Contracts;

namespace TickerSubscriptionDemo.Tests.UnitTests.Application.Subscriptions.Orchestrators;

[Trait("Category", "UnitTests")]
public class SubscriptionOrchestratorTests
{
    private readonly Mock<IDataService> dataServiceMock;
    private readonly Mock<ISubscriptionHandler> subscriptionHandlerMock;
    private readonly Mock<ISubscriptionRequestTransformer<Instrument>> transformerMock;

    private readonly ISubscriptionOrchestrator serviceUnderTest;

    public SubscriptionOrchestratorTests()
    {
        this.dataServiceMock = new Mock<IDataService>();
        this.subscriptionHandlerMock = new Mock<ISubscriptionHandler>();
        this.transformerMock = new Mock<ISubscriptionRequestTransformer<Instrument>>();

        this.serviceUnderTest = new SubscriptionOrchestrator(
            this.dataServiceMock.Object,
            this.subscriptionHandlerMock.Object,
            this.transformerMock.Object);
    }

    [Fact]
    public void Construction_WithNullDataService_ShouldThrow()
    {
        var construction = () => new SubscriptionOrchestrator(
            dataService: null!,
            Mock.Of<ISubscriptionHandler>(),
            Mock.Of<ISubscriptionRequestTransformer<Instrument>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("dataService");
    }

    [Fact]
    public void Construction_WithNullSubscriptionHandler_ShouldThrow()
    {
        var construction = () => new SubscriptionOrchestrator(
            Mock.Of<IDataService>(),
            subscriptionHandler: null!,
            Mock.Of<ISubscriptionRequestTransformer<Instrument>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("subscriptionHandler");
    }

    [Fact]
    public void Construction_WithNullRequestTransformer_ShouldThrow()
    {
        var construction = () => new SubscriptionOrchestrator(
            Mock.Of<IDataService>(),
            Mock.Of<ISubscriptionHandler>(),
            requestTransformer: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("requestTransformer");
    }

    [Fact]
    public async Task Start_ShouldSubscribeToAvailableInstruments()
    {
        var currencies = new[]
        {
            new Currency("ABC"),
            new Currency("XYZ")
        };

        this.dataServiceMock
            .Setup(m => m.GetCurrencies(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies);

        var instruments = new[]
        {
            new Instrument("ABC-MY-INS"),
            new Instrument("XYZ-MY-OTH")
        };

        this.dataServiceMock
            .Setup(m => m.GetInstrumentsForCurrency(currencies[0], It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]{ instruments[0] });

        this.dataServiceMock
            .Setup(m => m.GetInstrumentsForCurrency(currencies[1], It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]{ instruments[1] });

        var requests = new[]
        {
            new SubscriptionRequest(SubscriptionType.Ticker, "ABC-MY-INS", 100),
            new SubscriptionRequest(SubscriptionType.Ticker, "XYZ-MY-OTH", 100)
        };

        this.transformerMock
            .Setup(m => m.FromModels(instruments, SubscriptionType.Ticker))
            .Returns(requests);

        await this.serviceUnderTest.Start(CancellationToken.None);

        this.dataServiceMock.Verify(m =>
            m.Connect(It.IsAny<CancellationToken>()),
            Times.Once);

        this.dataServiceMock.Verify(m =>
            m.Subscribe(requests, this.subscriptionHandlerMock.Object, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Stop_ShouldUnsubscribeFromAll()
    {
        await this.serviceUnderTest.Stop(CancellationToken.None);

        this.dataServiceMock.Verify(m =>
            m.UnsubscribeAll(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}