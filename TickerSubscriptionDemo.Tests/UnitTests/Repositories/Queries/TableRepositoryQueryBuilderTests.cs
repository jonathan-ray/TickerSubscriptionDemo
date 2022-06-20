using TickerSubscriptionDemo.Repositories.Queries;

namespace TickerSubscriptionDemo.Tests.UnitTests.Repositories.Queries;

[Trait("Category", "UnitTests")]
public class TableRepositoryQueryBuilderTests
{
    [Fact]
    public void Build_WithoutFilters_ShouldReturnEmpty()
    {
        var builder = new TableRepositoryQueryBuilder();

        var query = builder.Build();

        query.FilterValue.Should().Be(string.Empty);
    }

    [Fact]
    public void Build_WithPartitionKeyEqualsToFilter_ShouldReturnFilter()
    {
        var builder = new TableRepositoryQueryBuilder()
            .WithPartitionKeyEqualTo("abc");

        var query = builder.Build();

        query.FilterValue.Should().Be("PartitionKey eq 'abc'");
    }

    [Fact]
    public void Build_WithMultiplePartitionKeyEqualsToFilter_ShouldReturnLatestFilter()
    {
        var builder = new TableRepositoryQueryBuilder()
            .WithPartitionKeyEqualTo("abc")
            .WithPartitionKeyEqualTo("xyz");

        var query = builder.Build();

        query.FilterValue.Should().Be("PartitionKey eq 'xyz'");
    }
}