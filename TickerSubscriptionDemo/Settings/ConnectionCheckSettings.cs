namespace TickerSubscriptionDemo.Settings;

/// <summary>
/// Settings for Connection Checking
/// </summary>
public class ConnectionCheckSettings
{
    /// <summary>
    /// Interval period for checking the connection.
    /// </summary>
    public TimeSpan CheckIntervalPeriod { get; set; }
}