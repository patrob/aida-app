using Aida.Api.Subscriptions;
using Aida.Api.Subscriptions.Configuration;
using Aida.Api.Testing.Subscriptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Aida.Api.IntegrationTests;

public class CustomWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Stripe:ApiKey"] = "sk_test_mock",
                ["Stripe:WebhookSecret"] = "whsec_mock"
            });
        });
        
        builder.ConfigureServices(services =>
        {
            // Remove all real Stripe service implementations
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(IStripeAdapter) ||
                           d.ServiceType == typeof(IStripeSubscriptionService) ||
                           d.ServiceType == typeof(IStripeCustomerService) ||
                           d.ServiceType == typeof(IStripePaymentMethodService))
                .ToList();
                
            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }
            
            // Add the mock StripeAdapter - this is the primary service actually used by the API
            services.AddTransient<IStripeAdapter, MockStripeAdapter>();
            
            // Only add the other mock implementations if they're actually needed directly
            // For the integration tests, we can rely solely on the MockStripeAdapter
            // which doesn't depend on the problematic Stripe SDK classes
        });
        
        base.ConfigureWebHost(builder);
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<CustomWebFactory>;