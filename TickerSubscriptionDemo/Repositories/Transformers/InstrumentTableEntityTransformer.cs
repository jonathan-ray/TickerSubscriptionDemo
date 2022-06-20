using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories.Models;

namespace TickerSubscriptionDemo.Repositories.Transformers;

public class InstrumentTableEntityTransformer : ITableEntityTransformer<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>
{
    public InstrumentSubscriptionEntity FromModel(InstrumentSubscriptionSnapshot model)
    {
        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        return new InstrumentSubscriptionEntity
        {
            PartitionKey = model.Name,
            RowKey = model.Timestamp.Ticks.ToString(),
            Data = model.Data
        };
    }

    public InstrumentSubscriptionSnapshot FromEntity(InstrumentSubscriptionEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new InstrumentSubscriptionSnapshot(
            entity.PartitionKey,
            new DateTime(long.Parse(entity.RowKey)),
            entity.Data);
    }
}