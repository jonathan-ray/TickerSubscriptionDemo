using Microsoft.Extensions.Options;
using TickerSubscriptionDemo.Application.Subscriptions.Models;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Settings;

namespace TickerSubscriptionDemo.Application.Subscriptions.Transformers;

public class InstrumentSubscriptionRequestTransformer : ISubscriptionRequestTransformer<Instrument>
{
    private readonly SubscriptionSettings settings;

    public InstrumentSubscriptionRequestTransformer(IOptions<SubscriptionSettings> settings)
    {
        this.settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public IReadOnlyList<SubscriptionRequest> FromModels(IEnumerable<Instrument> models, SubscriptionType type)
    {
        if (models is null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        return models
            .Select(instrument => new SubscriptionRequest(type, instrument.Name, this.settings.RequestIntervalInMs))
            .ToList();
    }
}