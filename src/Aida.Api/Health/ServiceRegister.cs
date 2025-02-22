using Microsoft.Extensions.DependencyInjection;

namespace Aida.Api.Health;

public static class ServiceRegister
{
    public static IServiceCollection AddHealthFeature(this IServiceCollection services)
    {
        return services.AddTransient<IHealthService, HealthService>();
    }
}