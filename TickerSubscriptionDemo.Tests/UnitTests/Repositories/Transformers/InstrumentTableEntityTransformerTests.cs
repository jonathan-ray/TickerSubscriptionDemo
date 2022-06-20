using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories.Models;
using TickerSubscriptionDemo.Repositories.Transformers;

namespace TickerSubscriptionDemo.Tests.UnitTests.Repositories.Transformers;

[Trait("Category", "UnitTests")]
public class InstrumentTableEntityTransformerTests
{
    private readonly InstrumentTableEntityTransformer transformerUnderTest;

    public InstrumentTableEntityTransformerTests()
    {
        this.transformerUnderTest = new InstrumentTableEntityTransformer();
    }

    [Fact]
    public void FromModel_WithNullModel_ShouldThrow()
    {
        this.transformerUnderTest
            .Invoking(t => t.FromModel(model: null!))
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("model");
    }

    [Fact]
    public void FromModel_WithModel_ShouldReturnEntity()
    {
        var model = new InstrumentSubscriptionSnapshot("ABC-NAME", DateTime.UtcNow, "{ \"MY\": \"DATA\" }");

        var actualEntity = this.transformerUnderTest.FromModel(model);

        actualEntity.Should().NotBeNull();
        actualEntity.PartitionKey.Should().Be(model.Name);
        actualEntity.RowKey.Should().Be(model.Timestamp.Ticks.ToString());
        actualEntity.Data.Should().Be(model.Data);
    }

    [Fact]
    public void FromEntity_WithNullEntity_ShouldThrow()
    {
        this.transformerUnderTest
            .Invoking(t => t.FromEntity(entity: null!))
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("entity");
    }

    [Fact]
    public void FromEntity_WithEntity_ShouldReturnModel()
    {
        var timestamp = DateTime.UtcNow;
        var entity = new InstrumentSubscriptionEntity
        {
            PartitionKey = "ABC-NAME",
            RowKey = timestamp.Ticks.ToString(),
            Data = "{ \"MY\": \"DATA\" }"
        };

        var actualModel = this.transformerUnderTest.FromEntity(entity);

        actualModel.Should().NotBeNull();
        actualModel.Name.Should().Be(entity.PartitionKey);
        actualModel.Timestamp.Should().Be(timestamp);
        actualModel.Data.Should().Be(entity.Data);
    }
}