namespace Aida.Api.Subscriptions.Configuration;

public class StripeOptions
{
    public const string SectionName = "Stripe";
    
    public required string ApiKey { get; set; }
    public required string WebhookSecret { get; set; }
} 