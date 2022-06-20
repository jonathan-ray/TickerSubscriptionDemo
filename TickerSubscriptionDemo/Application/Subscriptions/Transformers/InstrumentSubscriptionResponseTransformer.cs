using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Domain.Models;

namespace TickerSubscriptionDemo.Application.Subscriptions.Transformers;

public class InstrumentSubscriptionResponseTransformer : ISubscriptionResponseTransformer<InstrumentSubscriptionSnapshot>
{
    public InstrumentSubscriptionSnapshot FromJson(JToken response)
    {
        if (response is null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var name = response["data"]?["instrument_name"]?.Value<string>();
        if (name is null)
        {
            throw new InvalidDataException("Instrument Name is not present.");
        }

        var unixTimeInMilliseconds = response["data"]?["timestamp"]?.Value<long>();
        if (!unixTimeInMilliseconds.HasValue)
        {
            throw new InvalidDataException("Instrument Subscription Timestamp is not present.");
        }
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeInMilliseconds.Value).UtcDateTime;

        return new InstrumentSubscriptionSnapshot(name, timestamp, response.ToString());
    }
}