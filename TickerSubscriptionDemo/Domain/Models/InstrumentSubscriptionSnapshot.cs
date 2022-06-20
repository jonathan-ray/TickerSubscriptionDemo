namespace TickerSubscriptionDemo.Domain.Models;

/// <summary>
/// Representation of a single Instrument subscription response.
/// </summary>
/// <param name="Name">Name of the Instrument.</param>
/// <param name="Timestamp">Time stamp of data received.</param>
/// <param name="Data">The snapshot data.</param>
public record InstrumentSubscriptionSnapshot(string Name, DateTime Timestamp, string Data);