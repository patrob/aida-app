using System.Threading.Tasks;
using Aida.Api.Subscriptions.Models;
using FluentValidation;

namespace Aida.Api.Subscriptions.Handlers;

public interface ICreateSubscriptionHandler
{
    Task<Subscription> HandleAsync(CreateSubscriptionRequest request);
}

public class CreateSubscriptionHandler(
    ISubscriptionService subscriptionService,
    IValidator<CreateSubscriptionRequest> createSubscriptionValidator)
    : ICreateSubscriptionHandler
{
    public async Task<Subscription> HandleAsync(CreateSubscriptionRequest request)
    {
        // Validate the request
        await createSubscriptionValidator.ValidateAndThrowAsync(request);
        
        // Create the subscription
        var subscription = await subscriptionService.CreateSubscriptionAsync(request);
        
        return subscription;
    }
} 