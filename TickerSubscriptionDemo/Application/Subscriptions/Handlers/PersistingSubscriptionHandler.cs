using Newtonsoft.Json.Linq;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Repositories;

namespace TickerSubscriptionDemo.Application.Subscriptions.Handlers;

public class PersistingSubscriptionHandler<TSubscriptionModel> : ISubscriptionHandler
{
    private readonly IRepository<TSubscriptionModel> repository;
    private readonly ISubscriptionResponseTransformer<TSubscriptionModel> subscriptionTransformer;

    public PersistingSubscriptionHandler(
        IRepository<TSubscriptionModel> repository,
        ISubscriptionResponseTransformer<TSubscriptionModel> subscriptionTransformer)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.subscriptionTransformer = subscriptionTransformer ?? throw new ArgumentNullException(nameof(subscriptionTransformer));
    }

    public async Task OnResponseReceived(JToken response)
    {
        var model = this.subscriptionTransformer.FromJson(response);
        await this.repository.Add(model);
    }
}