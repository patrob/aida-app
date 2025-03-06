using System.Threading.Tasks;
using Stripe;

namespace Aida.Api.Subscriptions;

public interface IStripeSubscriptionService
{
    Task<Subscription> CreateAsync(SubscriptionCreateOptions options);
    Task<Subscription> GetAsync(string subscriptionId);
    Task<Subscription> CancelAsync(string subscriptionId, SubscriptionCancelOptions options);
} 