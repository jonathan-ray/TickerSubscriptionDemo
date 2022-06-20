using Azure;
using Azure.Data.Tables;

namespace TickerSubscriptionDemo.Repositories.Models;

/// <summary>
/// Table entity for an instrument subscription snapshot.
/// </summary>
public class InstrumentSubscriptionEntity : ITableEntity
{
    /// <summary>
    /// Instrument Name.
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// Time stamp the data was received, in ticks.
    /// </summary>
    public string RowKey { get; set; }

    /// <summary>
    /// Last modified time stamp.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Used by Azure to ensure concurrent operations.
    /// </summary>
    public ETag ETag { get; set; }

    /// <summary>
    /// Full JSON data of the instrument subscription snapshot.
    /// </summary>
    public string Data { get; set; }
}