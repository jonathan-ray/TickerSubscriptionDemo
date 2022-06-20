using Microsoft.Extensions.Options;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Settings;
using TickerSubscriptionDemo.Utilities.Factories;

namespace TickerSubscriptionDemo.Tests.UnitTests.Utilities.Factories;

[Trait("Category", "UnitTests")]
public class ConnectionCheckHandlerFactoryTests
{
    [Fact]
    public void Construction_WithNullSettings_ShouldThrow()
    {
        var construction = () => new ConnectionCheckHandlerFactory(
            settings: Options.Create<ConnectionCheckSettings>(null!));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("settings");
    }

    [Fact]
    public void Create_WithValidParameters_ShouldCreateSuccessfully()
    {
        var factory = new ConnectionCheckHandlerFactory(Options.Create(new ConnectionCheckSettings()));

        var handler = factory.Create(
            Mock.Of<ITestConnectionService>(),
            Mock.Of<IConnectableService>());

        handler.Should().NotBeNull();
    }
}