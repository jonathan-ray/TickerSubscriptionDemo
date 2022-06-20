using TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;
using TickerSubscriptionDemo.Application.Subscriptions.Services;

namespace TickerSubscriptionDemo.Tests.UnitTests.Application.Subscriptions.Services;

[Trait("Category", "UnitTests")]
public class SubscriptionBackgroundServiceTests
{
    private readonly Mock<ISubscriptionOrchestrator> orchestratorMock;

    private readonly SubscriptionBackgroundService serviceUnderTest;

    public SubscriptionBackgroundServiceTests()
    {
        this.orchestratorMock = new Mock<ISubscriptionOrchestrator>();

        this.serviceUnderTest = new SubscriptionBackgroundService(this.orchestratorMock.Object);
    }

    [Fact]
    public void Construction_WithNullSubscriptionOrchestrator_ShouldThrow()
    {
        var construction = () => new SubscriptionBackgroundService(
            subscriptionOrchestrator: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("subscriptionOrchestrator");
    }

    [Fact]
    public async Task Start_ShouldStartOrchestrator()
    {
        await this.serviceUnderTest.StartAsync(CancellationToken.None);

        this.orchestratorMock.Verify(m =>
            m.Start(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Stop_ShouldStopOrchestrator()
    {
        await this.serviceUnderTest.StopAsync(CancellationToken.None);

        this.orchestratorMock.Verify(m =>
            m.Stop(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}