namespace TickerSubscriptionDemo.Repositories.Queries;

/// <summary>
/// Abstraction of a repository query.
/// </summary>
public interface IRepositoryQuery
{
    /// <summary>
    /// The query filter.
    /// </summary>
    string FilterValue { get; }
}