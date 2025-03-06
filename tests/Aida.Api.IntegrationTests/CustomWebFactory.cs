using Aida.Api.Subscriptions;
using Aida.Api.Subscriptions.Configuration;
using Aida.Api.Testing.Subscriptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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
            // Remove the real StripeAdapter registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IStripeAdapter));
                
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // Add the mock StripeAdapter
            services.AddTransient<IStripeAdapter, MockStripeAdapter>();
        });
        
        base.ConfigureWebHost(builder);
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<CustomWebFactory>;