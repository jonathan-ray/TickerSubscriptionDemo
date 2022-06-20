using Azure.Data.Tables;

namespace TickerSubscriptionDemo.Repositories;

/// <summary>
/// Facade for abstracting away the Azure TableClient instance.
/// </summary>
/// <typeparam name="TEntity">The entity type representing a row in the table.</typeparam>
public interface ITableClientFacade<TEntity> where TEntity : class, ITableEntity, new()
{
    /// <summary>
    /// Adds an entity to the table storage.
    /// </summary>
    /// <param name="entity">The table entity.</param>
    Task Add(TEntity entity);

    /// <summary>
    /// Queries over the table using an optional filter.
    /// </summary>
    /// <param name="filter">The filter to use.</param>
    /// <returns>The potentially filtered collection of entities.</returns>
    Task<IReadOnlyList<TEntity>> Query(string? filter = null);
}