namespace TickerSubscriptionDemo.Repositories.Queries;

/// <summary>
/// Builder class for table repository queries.
/// </summary>
public class TableRepositoryQueryBuilder
{
    private const string PartitionKeyField = "PartitionKey";

    private readonly IDictionary<string, string> filtersByField = new Dictionary<string, string>();

    /// <summary>
    /// Adds a partition key is equal to filter.
    /// </summary>
    /// <param name="partitionKey">The partition key.</param>
    public TableRepositoryQueryBuilder WithPartitionKeyEqualTo(string partitionKey)
    {
        this.filtersByField[PartitionKeyField] = $"eq '{partitionKey}'";

        return this;
    }

    /// <summary>
    /// Builds the table repository query.
    /// </summary>
    /// <returns>The table repository query.</returns>
    public TableRepositoryQuery Build()
    {
        var filterValue = string.Join(" and ", this.filtersByField.Select(filters => $"{filters.Key} {filters.Value}"));
        return new TableRepositoryQuery(filterValue);
    }
}