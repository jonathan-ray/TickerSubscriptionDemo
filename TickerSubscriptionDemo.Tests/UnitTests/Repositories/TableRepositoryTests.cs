using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories;
using TickerSubscriptionDemo.Repositories.Models;
using TickerSubscriptionDemo.Repositories.Queries;
using TickerSubscriptionDemo.Repositories.Transformers;

namespace TickerSubscriptionDemo.Tests.UnitTests.Repositories;

[Trait("Category", "UnitTests")]
public class TableRepositoryTests
{
    private readonly Mock<ITableClientFacade<InstrumentSubscriptionEntity>> tableClientMock;
    private readonly Mock<ITableEntityTransformer<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>> transformerMock;

    private readonly TableRepository<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity> repositoryUnderTest;

    public TableRepositoryTests()
    {
        this.tableClientMock = new Mock<ITableClientFacade<InstrumentSubscriptionEntity>>();
        this.transformerMock = new Mock<ITableEntityTransformer<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>>();

        this.repositoryUnderTest = new TableRepository<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>(
            this.tableClientMock.Object,
            this.transformerMock.Object);
    }

    [Fact]
    public void Construction_WithNullTableClient_ShouldThrow()
    {
        var construction = () => new TableRepository<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>(
            tableClient: null!,
            Mock.Of<ITableEntityTransformer<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("tableClient");
    }

    [Fact]
    public void Construction_WithNullEntityTransformer_ShouldThrow()
    {
        var construction = () => new TableRepository<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>(
            Mock.Of<ITableClientFacade<InstrumentSubscriptionEntity>>(),
            entityTransformer: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("entityTransformer");
    }

    [Fact]
    public async Task Add_WithModel_ShouldAddEntity()
    {
        var model = new InstrumentSubscriptionSnapshot("AB", DateTime.UtcNow, "DATA");
        var entity = new InstrumentSubscriptionEntity();

        this.transformerMock
            .Setup(m => m.FromModel(model))
            .Returns(entity);

        await this.repositoryUnderTest.Add(model);

        this.tableClientMock.Verify(m =>
            m.Add(entity),
            Times.Once);
    }

    [Fact]
    public async Task Query_WithoutFilter_ShouldReturnData()
    {
        var entities = new[]
        {
            new InstrumentSubscriptionEntity()
        };

        this.tableClientMock
            .Setup(m => m.Query(null))
            .ReturnsAsync(entities);

        var expectedModel = new InstrumentSubscriptionSnapshot("AB", DateTime.UtcNow, "DATA");

        this.transformerMock
            .Setup(m => m.FromEntity(entities[0]))
            .Returns(expectedModel);

        var actualModels = await this.repositoryUnderTest.Query(null);

        actualModels.Should().HaveCount(1);
        actualModels[0].Should().Be(expectedModel);
    }

    [Fact]
    public async Task Query_WithFilter_ShouldReturnData()
    {
        var query = new TableRepositoryQuery("filterValue");

        var entities = new[]
        {
            new InstrumentSubscriptionEntity()
        };

        this.tableClientMock
            .Setup(m => m.Query(query.FilterValue))
            .ReturnsAsync(entities);

        var expectedModel = new InstrumentSubscriptionSnapshot("AB", DateTime.UtcNow, "DATA");

        this.transformerMock
            .Setup(m => m.FromEntity(entities[0]))
            .Returns(expectedModel);

        var actualModels = await this.repositoryUnderTest.Query(query);

        actualModels.Should().HaveCount(1);
        actualModels[0].Should().Be(expectedModel);
    }

    [Fact]
    public async Task Query_WithoutData_ShouldReturnEmpty()
    {
        this.tableClientMock
            .Setup(m => m.Query(null))
            .ReturnsAsync(Array.Empty<InstrumentSubscriptionEntity>());

        var actualModels = await this.repositoryUnderTest.Query(null);

        actualModels.Should().BeEmpty();
    }
}