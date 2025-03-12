namespace Aida.Api.Subscriptions.Models;

public class CreateSubscriptionRequest
{
    public required string CustomerId { get; set; }
    public required string PlanId { get; set; }
    public required string PaymentMethodId { get; set; }
} 