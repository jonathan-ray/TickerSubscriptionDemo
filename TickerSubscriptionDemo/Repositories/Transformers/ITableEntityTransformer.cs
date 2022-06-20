using Azure.Data.Tables;

namespace TickerSubscriptionDemo.Repositories.Transformers;

/// <summary>
/// Transformer for converting between internal models and table entities.
/// </summary>
/// <typeparam name="TModel">Internal model type.</typeparam>
/// <typeparam name="TEntity">Table entity type.</typeparam>
public interface ITableEntityTransformer<TModel, TEntity> where TEntity : ITableEntity
{
    /// <summary>
    /// Creates a table entity from a given internal model.
    /// </summary>
    /// <param name="model">The internal model.</param>
    /// <returns>The table entity equivalent.</returns>
    TEntity FromModel(TModel model);

    /// <summary>
    /// Creates an internal model from a given table entity.
    /// </summary>
    /// <param name="entity">The table entity.</param>
    /// <returns>The internal model equivalent.</returns>
    TModel FromEntity(TEntity entity);
}