using TickerSubscriptionDemo.Controllers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories;
using TickerSubscriptionDemo.Repositories.Queries;

namespace TickerSubscriptionDemo.Tests.UnitTests.Controllers;

[Trait("Category", "UnitTests")]
public class InstrumentsControllerTests
{
    private readonly Mock<IRepository<InstrumentSubscriptionSnapshot>> repositoryMock;

    private readonly InstrumentsController controllerUnderTest;

    public InstrumentsControllerTests()
    {
        this.repositoryMock = new Mock<IRepository<InstrumentSubscriptionSnapshot>>();

        this.controllerUnderTest = new InstrumentsController(this.repositoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new InstrumentsController(repository: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   \t")]
    public async Task GetInstrumentSnapshots_WithNullOrWhitespaceInstrumentName_ShouldThrow(string? instrumentName)
    {
        var exceptionAssertion = await this.controllerUnderTest
            .Awaiting(c => c.GetInstrumentSnapshots(instrumentName: instrumentName!))
            .Should().ThrowAsync<ArgumentNullException>();

        exceptionAssertion.And.ParamName.Should().Be("instrumentName");
    }

    [Fact]
    public async Task GetInstrumentSnapshots_WithData_ShouldReturnSuccessfully()
    {
        var jsonData = 
@"{
  ""abc"": ""123""
}";

        var data = new[]
        {
            new InstrumentSubscriptionSnapshot("ABC-001", DateTime.UtcNow, jsonData)
        };

        this.repositoryMock
            .Setup(m => m.Query(It.IsAny<IRepositoryQuery>()))
            .ReturnsAsync(data);

        var actualResults = await this.controllerUnderTest.GetInstrumentSnapshots("abc");

        var singleResult = actualResults.Should().ContainSingle().Subject;
        singleResult.ToString().Should().Contain(jsonData);
    }

    [Fact]
    public async Task GetInstrumentSnapshots_WithoutData_ShouldReturnSuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.Query(It.IsAny<IRepositoryQuery>()))
            .ReturnsAsync(Array.Empty<InstrumentSubscriptionSnapshot>());

        var actualResults = await this.controllerUnderTest.GetInstrumentSnapshots("abc");

        actualResults.Should().BeEmpty();
    }
}