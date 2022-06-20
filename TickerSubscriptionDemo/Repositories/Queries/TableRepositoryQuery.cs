namespace TickerSubscriptionDemo.Repositories.Queries;

/// <summary>
/// Table repository query.
/// </summary>
/// <param name="FilterValue">The query filter.</param>
public record TableRepositoryQuery(string FilterValue) : IRepositoryQuery;