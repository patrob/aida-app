using System.Threading.Tasks;
using Stripe;

namespace Aida.Api.Subscriptions;

public class StripeSubscriptionServiceImpl : IStripeSubscriptionService
{
    private readonly ISubscriptionService _subscriptionService;

    public StripeSubscriptionServiceImpl(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public Task<Subscription> CreateAsync(SubscriptionCreateOptions options)
    {
        return _subscriptionService.CreateSubscriptionAsync(options);
    }

    public Task<Subscription> GetAsync(string subscriptionId)
    {
        return _subscriptionService.GetSubscriptionAsync(subscriptionId);
    }

    public Task<Subscription> CancelAsync(string subscriptionId, SubscriptionCancelOptions options)
    {
        return _subscriptionService.CancelSubscriptionAsync(subscriptionId, options);
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