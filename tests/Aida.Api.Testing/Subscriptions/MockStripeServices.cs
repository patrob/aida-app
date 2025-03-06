using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aida.Api.Subscriptions;
using Stripe;

namespace Aida.Api.Testing.Subscriptions;

public class MockStripeSubscriptionService : IStripeSubscriptionService
{
    private readonly Dictionary<string, Subscription> _subscriptions = new();
    
    public Task<Subscription> CreateAsync(SubscriptionCreateOptions options)
    {
        var subscription = new Subscription
        {
            Id = $"sub_{System.Guid.NewGuid():N}",
            CustomerId = options.Customer,
            Items = new StripeList<SubscriptionItem>
            {
                Data = new List<SubscriptionItem>
                {
                    new()
                    {
                        Price = new Price { Id = options.Items[0].Price }
                    }
                }
            },
            Status = "active",
            CurrentPeriodEnd = System.DateTime.UtcNow.AddMonths(1)
        };
        
        _subscriptions[subscription.Id] = subscription;
        return Task.FromResult(subscription);
    }

    public Task<Subscription> GetAsync(string subscriptionId)
    {
        if (_subscriptions.TryGetValue(subscriptionId, out var subscription))
        {
            return Task.FromResult(subscription);
        }
        
        throw CreateNotFoundStripeException(subscriptionId);
    }

    public Task<Subscription> CancelAsync(string subscriptionId, SubscriptionCancelOptions options)
    {
        if (_subscriptions.TryGetValue(subscriptionId, out var subscription))
        {
            subscription.Status = "canceled";
            return Task.FromResult(subscription);
        }
        
        throw CreateNotFoundStripeException(subscriptionId);
    }
    
    private StripeException CreateNotFoundStripeException(string subscriptionId) => new(
        HttpStatusCode.NotFound,
        new StripeError
        {
            Type = "invalid_request_error",
            Code = "resource_missing",
            Message = $"No such subscription: {subscriptionId}"
        },
        $"No such subscription: {subscriptionId}"
    );
}

public class MockStripeCustomerService : IStripeCustomerService
{
    public Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options)
    {
        return Task.FromResult(new Customer
        {
            Id = customerId,
            InvoiceSettings = new CustomerInvoiceSettings
            {
                DefaultPaymentMethod = options.InvoiceSettings?.DefaultPaymentMethod
            }
        });
    }
}

public class MockStripePaymentMethodService : IStripePaymentMethodService
{
    public Task<PaymentMethod> AttachAsync(string paymentMethodId, PaymentMethodAttachOptions options)
    {
        return Task.FromResult(new PaymentMethod
        {
            Id = paymentMethodId,
            CustomerId = options.Customer
        });
    }
} 