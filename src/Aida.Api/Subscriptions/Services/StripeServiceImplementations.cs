using System.Collections.Generic;
using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using Stripe;
using StripeSubscription = Stripe.Subscription;
using AppSubscription = Aida.Api.Subscriptions.Models.Subscription;

namespace Aida.Api.Subscriptions;

public class StripeSubscriptionServiceImpl : IStripeSubscriptionService
{
    private readonly Stripe.SubscriptionService _stripeSubscriptionService;

    public StripeSubscriptionServiceImpl()
    {
        _stripeSubscriptionService = new Stripe.SubscriptionService();
    }

    public Task<StripeSubscription> CreateAsync(SubscriptionCreateOptions options)
    {
        return _stripeSubscriptionService.CreateAsync(options);
    }

    public Task<StripeSubscription> GetAsync(string subscriptionId)
    {
        return _stripeSubscriptionService.GetAsync(subscriptionId);
    }

    public Task<StripeSubscription> CancelAsync(string subscriptionId, SubscriptionCancelOptions options)
    {
        return _stripeSubscriptionService.CancelAsync(subscriptionId, options);
    }
}

public class StripeCustomerServiceImpl : IStripeCustomerService
{
    private readonly CustomerService _customerService;

    public StripeCustomerServiceImpl()
    {
        _customerService = new CustomerService();
    }

    public Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options)
    {
        return _customerService.UpdateAsync(customerId, options);
    }
}

public class StripePaymentMethodServiceImpl : IStripePaymentMethodService
{
    private readonly PaymentMethodService _paymentMethodService;

    public StripePaymentMethodServiceImpl()
    {
        _paymentMethodService = new PaymentMethodService();
    }

    public Task<PaymentMethod> AttachAsync(string paymentMethodId, PaymentMethodAttachOptions options)
    {
        return _paymentMethodService.AttachAsync(paymentMethodId, options);
    }
} 