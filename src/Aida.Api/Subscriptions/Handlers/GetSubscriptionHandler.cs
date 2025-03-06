using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using Aida.Api.Subscriptions.Validators;
using FluentValidation;

namespace Aida.Api.Subscriptions.Handlers;

public interface IGetSubscriptionHandler
{
    Task<Subscription> HandleAsync(string subscriptionId);
}

public class GetSubscriptionHandler : IGetSubscriptionHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IValidator<string> _subscriptionIdValidator;

    public GetSubscriptionHandler(
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
        
        // Get the subscription from the service
        var subscription = await _subscriptionService.GetSubscriptionAsync(subscriptionId);
        
        // Throw KeyNotFoundException if subscription doesn't exist
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {subscriptionId} not found");
        }
        
        return subscription;
    }
} 