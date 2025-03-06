using System.Threading.Tasks;
using Aida.Api.Subscriptions.Handlers;
using Aida.Api.Subscriptions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aida.Api.Subscriptions;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController(
    ICreateSubscriptionHandler createSubscriptionHandler,
    IGetSubscriptionHandler getSubscriptionHandler,
    ICancelSubscriptionHandler cancelSubscriptionHandler)
    : ControllerBase
{
    [HttpPost]
    public async Task<Subscription> CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        return await createSubscriptionHandler.HandleAsync(request);
    }

    [HttpGet("{subscriptionId}")]
    public async Task<Subscription> GetSubscription(string subscriptionId)
    {
        return await getSubscriptionHandler.HandleAsync(subscriptionId);
    }

    [HttpDelete("{subscriptionId}")]
    public async Task<Subscription> CancelSubscription(string subscriptionId)
    {
        return await cancelSubscriptionHandler.HandleAsync(subscriptionId);
    }
} 