using Microsoft.Extensions.Options;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Settings;

namespace TickerSubscriptionDemo.Tests.UnitTests.Application.Subscriptions.Transformers;

[Trait("Category", "UnitTests")]
public class InstrumentSubscriptionRequestTransformerTests
{
    private static readonly SubscriptionSettings DefaultSettings = new() { RequestIntervalInMs = 100 };

    private readonly InstrumentSubscriptionRequestTransformer transformerUnderTest;

    public InstrumentSubscriptionRequestTransformerTests()
    {
        this.transformerUnderTest = new InstrumentSubscriptionRequestTransformer(Options.Create(DefaultSettings));
    }

    [Fact]
    public void Construction_WithNullSettings_ShouldThrow()
    {
        var construction = () => new InstrumentSubscriptionRequestTransformer(
            settings: Options.Create<SubscriptionSettings>(null!));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("settings");
    }

    [Fact]
    public void FromModels_WithNullModels_ShouldThrow()
    {
        this.transformerUnderTest
            .Invoking(t => t.FromModels(models: null!, SubscriptionType.Ticker))
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("models");
    }

    [Fact]
    public void FromModels_WithEmptyModels_ShouldReturnEmpty()
    {
        var requests = this.transformerUnderTest.FromModels(Array.Empty<Instrument>(), SubscriptionType.Ticker);

        requests.Should().BeEmpty();
    }

    [Fact]
    public void FromModels_WithModels_ShouldReturnEmpty()
    {
        const SubscriptionType expectedType = SubscriptionType.Ticker;

        var models = new[]
        {
            new Instrument("ABC-001"),
            new Instrument("XYZ-003")
        };

        var actualRequests = this.transformerUnderTest.FromModels(models, expectedType);

        actualRequests.Should().HaveCount(models.Length);
        for (var i = 0; i < models.Length; i++)
        {
            actualRequests[i].Name.Should().Be(models[i].Name);
            actualRequests[i].Type.Should().Be(expectedType);
            actualRequests[i].IntervalMs.Should().Be(DefaultSettings.RequestIntervalInMs);
        }
    }
}