using Azure.Data.Tables;

namespace TickerSubscriptionDemo.Repositories;

public class TableClientFacade<TEntity> : ITableClientFacade<TEntity> where TEntity : class, ITableEntity, new()
{
    private readonly TableClient tableClient;

    public TableClientFacade(TableClient tableClient)
    {
        this.tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
        this.tableClient.CreateIfNotExists();
    }

    public Task Add(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return this.tableClient.UpsertEntityAsync(entity);
    }

    public async Task<IReadOnlyList<TEntity>> Query(string? filter = null)
    {
        var results = new List<TEntity>();

        var entities = this.tableClient.QueryAsync<TEntity>(filter);
        await foreach (var entity in entities)
        {
            results.Add(entity);
        }

        return results;
    }
}