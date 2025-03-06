using System.Collections.Generic;
using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using Microsoft.Extensions.Logging;
using Stripe;
using SubscriptionModel = Aida.Api.Subscriptions.Models.Subscription;

namespace Aida.Api.Subscriptions;

public interface IStripeAdapter
{
    Task<SubscriptionModel> CreateSubscriptionAsync(string customerId, string planId, string paymentMethodId);
    Task<SubscriptionModel?> GetSubscriptionAsync(string subscriptionId);
    Task<SubscriptionModel?> CancelSubscriptionAsync(string subscriptionId);
}

public class StripeAdapter : IStripeAdapter
{
    private readonly ILogger<StripeAdapter> _logger;
    private readonly IStripeSubscriptionService _stripeSubscriptionService;
    private readonly IStripeCustomerService _stripeCustomerService;
    private readonly IStripePaymentMethodService _stripePaymentMethodService;

    public StripeAdapter(
        ILogger<StripeAdapter> logger,
        IStripeSubscriptionService stripeSubscriptionService,
        IStripeCustomerService stripeCustomerService,
        IStripePaymentMethodService stripePaymentMethodService)
    {
        _logger = logger;
        _stripeSubscriptionService = stripeSubscriptionService;
        _stripeCustomerService = stripeCustomerService;
        _stripePaymentMethodService = stripePaymentMethodService;
    }

    public async Task<SubscriptionModel> CreateSubscriptionAsync(string customerId, string planId, string paymentMethodId)
    {
        _logger.LogInformation("Creating Stripe subscription for customer {CustomerId} with plan {PlanId}", customerId, planId);
        
        try
        {
            // First, attach the payment method to the customer
            await _stripePaymentMethodService.AttachAsync(
                paymentMethodId,
                new PaymentMethodAttachOptions { Customer = customerId }
            );

            // Set the payment method as the default for the customer
            await _stripeCustomerService.UpdateAsync(
                customerId,
                new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId
                    }
                }
            );

            // Create the subscription
            var stripeSubscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
                {
                    new() { Price = planId }
                },
                PaymentSettings = new SubscriptionPaymentSettingsOptions
                {
                    PaymentMethodTypes = new List<string> { "card" }
                }
            };

            var stripeSubscription = await _stripeSubscriptionService.CreateAsync(stripeSubscriptionOptions);

            return MapStripeSubscriptionToModel(stripeSubscription);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Error creating Stripe subscription");
            throw;
        }
    }

    public async Task<SubscriptionModel?> GetSubscriptionAsync(string subscriptionId)
    {
        _logger.LogInformation("Fetching Stripe subscription {SubscriptionId}", subscriptionId);
        
        try
        {
            var stripeSubscription = await _stripeSubscriptionService.GetAsync(subscriptionId);
            return MapStripeSubscriptionToModel(stripeSubscription);
        }
        catch (StripeException ex)
        {
            if (ex.StripeError.Type == "invalid_request_error" && ex.StripeError.Code == "resource_missing")
            {
                _logger.LogWarning("Subscription {SubscriptionId} not found", subscriptionId);
                return null;
            }
            
            _logger.LogError(ex, "Error fetching Stripe subscription");
            throw;
        }
    }

    public async Task<SubscriptionModel?> CancelSubscriptionAsync(string subscriptionId)
    {
        _logger.LogInformation("Canceling Stripe subscription {SubscriptionId}", subscriptionId);
        
        try
        {
            var stripeSubscription = await _stripeSubscriptionService.CancelAsync(
                subscriptionId,
                new SubscriptionCancelOptions
                {
                    InvoiceNow = true,
                    Prorate = true
                }
            );
            
            return MapStripeSubscriptionToModel(stripeSubscription);
        }
        catch (StripeException ex)
        {
            if (ex.StripeError.Type == "invalid_request_error" && ex.StripeError.Code == "resource_missing")
            {
                _logger.LogWarning("Subscription {SubscriptionId} not found for cancellation", subscriptionId);
                return null;
            }
            
            _logger.LogError(ex, "Error canceling Stripe subscription");
            throw;
        }
    }

    private static SubscriptionModel MapStripeSubscriptionToModel(Stripe.Subscription stripeSubscription)
    {
        var status = stripeSubscription.Status switch
        {
            "active" => SubscriptionStatus.Active,
            "past_due" => SubscriptionStatus.PastDue,
            "canceled" => SubscriptionStatus.Canceled,
            "unpaid" => SubscriptionStatus.Unpaid,
            _ => SubscriptionStatus.Active
        };

        return new SubscriptionModel
        {
            Id = stripeSubscription.Id,
            CustomerId = stripeSubscription.CustomerId,
            PlanId = stripeSubscription.Items.Data[0].Price.Id,
            Status = status,
            CurrentPeriodEnd = stripeSubscription.CurrentPeriodEnd
        };
    }
} 