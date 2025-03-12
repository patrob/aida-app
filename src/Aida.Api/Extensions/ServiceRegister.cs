using Aida.Api.Health;
using Aida.Api.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aida.Api.Extensions;

public static class ServiceRegister
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddHealthFeature()
            .AddSubscriptionsFeature(configuration);
    }
}