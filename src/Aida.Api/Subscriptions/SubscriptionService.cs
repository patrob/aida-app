using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using Microsoft.Extensions.Logging;

namespace Aida.Api.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription> CreateSubscriptionAsync(CreateSubscriptionRequest request);
    Task<Subscription?> GetSubscriptionAsync(string subscriptionId);
    Task<Subscription?> CancelSubscriptionAsync(string subscriptionId);
}

public class SubscriptionService : ISubscriptionService
{
    private readonly IStripeAdapter _stripeAdapter;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(IStripeAdapter stripeAdapter, ILogger<SubscriptionService> logger)
    {
        _stripeAdapter = stripeAdapter;
        _logger = logger;
    }

    public async Task<Subscription> CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        _logger.LogInformation("Creating subscription for customer {CustomerId} with plan {PlanId}", 
            request.CustomerId, request.PlanId);
            
        var subscription = await _stripeAdapter.CreateSubscriptionAsync(
            request.CustomerId, 
            request.PlanId, 
            request.PaymentMethodId);
            
        _logger.LogInformation("Subscription {SubscriptionId} created successfully", subscription.Id);
        
        return subscription;
    }

    public async Task<Subscription?> GetSubscriptionAsync(string subscriptionId)
    {
        _logger.LogInformation("Getting subscription {SubscriptionId}", subscriptionId);
        
        var subscription = await _stripeAdapter.GetSubscriptionAsync(subscriptionId);
        
        if (subscription == null)
        {
            _logger.LogWarning("Subscription {SubscriptionId} not found", subscriptionId);
        }
        
        return subscription;
    }

    public async Task<Subscription?> CancelSubscriptionAsync(string subscriptionId)
    {
        _logger.LogInformation("Canceling subscription {SubscriptionId}", subscriptionId);
        
        var subscription = await _stripeAdapter.CancelSubscriptionAsync(subscriptionId);
        
        if (subscription == null)
        {
            _logger.LogWarning("Subscription {SubscriptionId} not found for cancellation", subscriptionId);
        }
        else
        {
            _logger.LogInformation("Subscription {SubscriptionId} canceled successfully", subscriptionId);
        }
        
        return subscription;
    }
} 