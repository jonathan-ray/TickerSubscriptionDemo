using TickerSubscriptionDemo.Repositories.Queries;

namespace TickerSubscriptionDemo.Repositories;

/// <summary>
/// Repository for storing a collection of specific internal models.
/// </summary>
/// <typeparam name="TModel">The internal model type.</typeparam>
public interface IRepository<TModel>
{
    /// <summary>
    /// Adds the model instance to the repository.
    /// </summary>
    /// <param name="model">The model instance.</param>
    Task Add(TModel model);

    /// <summary>
    /// Queries over the repository.
    /// </summary>
    /// <param name="queryFilter">An optional filter.</param>
    /// <returns>The collection of models that match the query.</returns>
    Task<IReadOnlyList<TModel>> Query(IRepositoryQuery? queryFilter);
}