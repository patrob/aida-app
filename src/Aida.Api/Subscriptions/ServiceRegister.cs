using Aida.Api.Subscriptions.Configuration;
using Aida.Api.Subscriptions.Handlers;
using Aida.Api.Subscriptions.Models;
using Aida.Api.Subscriptions.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;

namespace Aida.Api.Subscriptions;

public static class ServiceRegister
{
    public static IServiceCollection AddSubscriptionsFeature(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Stripe configuration
        services.Configure<StripeOptions>(
            configuration.GetSection(StripeOptions.SectionName));

        // Configure Stripe globally
        services.AddSingleton<IConfigureOptions<StripeOptions>>(serviceProvider =>
        {
            return new ConfigureNamedOptions<StripeOptions>(Options.DefaultName, options =>
            {
                var stripeSection = configuration.GetSection(StripeOptions.SectionName);
                stripeSection.Bind(options);
            });
        });

        // Set up Stripe API key from configuration
        services.AddSingleton<IPostConfigureOptions<StripeOptions>>(serviceProvider =>
        {
            return new PostConfigureOptions<StripeOptions>(Options.DefaultName, options =>
            {
                StripeConfiguration.ApiKey = options.ApiKey;
            });
        });

        // Register Stripe services
        services.AddTransient<IStripeSubscriptionService, StripeSubscriptionServiceImpl>();
        services.AddTransient<IStripeCustomerService, StripeCustomerServiceImpl>();
        services.AddTransient<IStripePaymentMethodService, StripePaymentMethodServiceImpl>();

        // Register validators
        services.AddScoped<IValidator<CreateSubscriptionRequest>, CreateSubscriptionRequestValidator>();
        services.AddScoped<IValidator<string>, SubscriptionIdValidator>();

        // Register services
        services.AddTransient<IStripeAdapter, StripeAdapter>();
        services.AddTransient<ISubscriptionService, SubscriptionService>();
        
        // Register handlers
        services.AddTransient<ICreateSubscriptionHandler, CreateSubscriptionHandler>();
        services.AddTransient<IGetSubscriptionHandler, GetSubscriptionHandler>();
        services.AddTransient<ICancelSubscriptionHandler, CancelSubscriptionHandler>();

        return services;
    }
} 