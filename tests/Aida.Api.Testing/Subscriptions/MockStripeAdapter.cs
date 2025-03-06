using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aida.Api.Subscriptions;
using Aida.Api.Subscriptions.Models;

namespace Aida.Api.Testing.Subscriptions;

public class MockStripeAdapter : IStripeAdapter
{
    private readonly Dictionary<string, Subscription> _subscriptions = new();

    public Task<Subscription> CreateSubscriptionAsync(string customerId, string planId, string paymentMethodId)
    {
        var subscription = new Subscription
        {
            Id = $"sub_{Guid.NewGuid():N}",
            CustomerId = customerId,
            PlanId = planId,
            Status = SubscriptionStatus.Active,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };

        _subscriptions[subscription.Id] = subscription;
        return Task.FromResult(subscription);
    }

    public Task<Subscription?> GetSubscriptionAsync(string subscriptionId)
    {
        if (_subscriptions.TryGetValue(subscriptionId, out var subscription))
        {
            return Task.FromResult<Subscription?>(subscription);
        }

        return Task.FromResult<Subscription?>(null);
    }

    public Task<Subscription?> CancelSubscriptionAsync(string subscriptionId)
    {
        if (_subscriptions.TryGetValue(subscriptionId, out var subscription))
        {
            subscription.Status = SubscriptionStatus.Canceled;
            return Task.FromResult<Subscription?>(subscription);
        }

        return Task.FromResult<Subscription?>(null);
    }
} 