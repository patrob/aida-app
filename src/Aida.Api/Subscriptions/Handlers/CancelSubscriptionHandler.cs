using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using Aida.Api.Subscriptions.Validators;
using FluentValidation;

namespace Aida.Api.Subscriptions.Handlers;

public interface ICancelSubscriptionHandler
{
    Task<Subscription> HandleAsync(string subscriptionId);
}

public class CancelSubscriptionHandler : ICancelSubscriptionHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IValidator<string> _subscriptionIdValidator;

    public CancelSubscriptionHandler(
        ISubscriptionService subscriptionService,
        IValidator<string> subscriptionIdValidator)
    {
        _subscriptionService = subscriptionService;
        _subscriptionIdValidator = subscriptionIdValidator;
    }

    public async Task<Subscription> HandleAsync(string subscriptionId)
    {
        // Validate the subscription ID
        await _subscriptionIdValidator.ValidateAndThrowAsync(subscriptionId);
        
        // Call the subscription service to cancel the subscription
        var subscription = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        
        // Throw KeyNotFoundException if subscription doesn't exist
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {subscriptionId} not found");
        }
        
        return subscription;
    }
} 