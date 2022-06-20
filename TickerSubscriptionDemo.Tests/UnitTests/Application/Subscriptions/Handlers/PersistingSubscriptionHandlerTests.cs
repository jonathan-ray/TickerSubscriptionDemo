using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories;

namespace TickerSubscriptionDemo.Tests.UnitTests.Application.Subscriptions.Handlers;

[Trait("Category", "UnitTests")]
public class PersistingSubscriptionHandlerTests
{
    private readonly Mock<IRepository<InstrumentSubscriptionSnapshot>> repositoryMock;
    private readonly Mock<ISubscriptionResponseTransformer<InstrumentSubscriptionSnapshot>> transformerMock;

    private readonly PersistingSubscriptionHandler<InstrumentSubscriptionSnapshot> handlerUnderTest;

    public PersistingSubscriptionHandlerTests()
    {
        this.repositoryMock = new Mock<IRepository<InstrumentSubscriptionSnapshot>>();
        this.transformerMock = new Mock<ISubscriptionResponseTransformer<InstrumentSubscriptionSnapshot>>();

        this.handlerUnderTest = new PersistingSubscriptionHandler<InstrumentSubscriptionSnapshot>(
            this.repositoryMock.Object,
            this.transformerMock.Object);
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new PersistingSubscriptionHandler<InstrumentSubscriptionSnapshot>(
            repository: null!,
            Mock.Of<ISubscriptionResponseTransformer<InstrumentSubscriptionSnapshot>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Fact]
    public void Construction_WithNullSubscriptionTransformer_ShouldThrow()
    {
        var construction = () => new PersistingSubscriptionHandler<InstrumentSubscriptionSnapshot>(
            Mock.Of<IRepository<InstrumentSubscriptionSnapshot>>(),
            subscriptionTransformer: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("subscriptionTransformer");
    }

    [Fact]
    public async Task OnResponseReceived_WithResponse_ShouldPersist()
    {
        var snapshot = new InstrumentSubscriptionSnapshot("abc", DateTime.UtcNow, "data");

        this.transformerMock
            .Setup(m => m.FromJson(It.IsAny<JToken>()))
            .Returns(snapshot);

        await this.handlerUnderTest.OnResponseReceived(JToken.Parse("{}"));

        this.transformerMock.Verify(m =>
            m.FromJson(It.IsAny<JToken>()),
            Times.Once);

        this.repositoryMock.Verify(m =>
            m.Add(snapshot),
            Times.Once);
    }
}