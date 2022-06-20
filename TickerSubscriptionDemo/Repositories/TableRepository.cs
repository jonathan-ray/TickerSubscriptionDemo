using Azure.Data.Tables;
using TickerSubscriptionDemo.Repositories.Queries;
using TickerSubscriptionDemo.Repositories.Transformers;

namespace TickerSubscriptionDemo.Repositories;

public class TableRepository<TModel, TEntity> : IRepository<TModel> where TEntity : class, ITableEntity, new()
{
    private readonly ITableClientFacade<TEntity> tableClient;
    private readonly ITableEntityTransformer<TModel, TEntity> entityTransformer;

    public TableRepository(
        ITableClientFacade<TEntity> tableClient,
        ITableEntityTransformer<TModel, TEntity> entityTransformer)
    {
        this.tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
        this.entityTransformer = entityTransformer ?? throw new ArgumentNullException(nameof(entityTransformer));
    }

    public async Task Add(TModel model)
    {
        var entity = this.entityTransformer.FromModel(model);
        await this.tableClient.Add(entity);
    }

    public async Task<IReadOnlyList<TModel>> Query(IRepositoryQuery? queryFilter)
    {
        var entities = await this.tableClient.Query(queryFilter?.FilterValue);
        var models = entities.Select(entity => this.entityTransformer.FromEntity(entity));
        return models.ToList();
    }
}