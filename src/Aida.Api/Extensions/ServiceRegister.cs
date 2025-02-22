using Aida.Api.Health;
using Microsoft.Extensions.DependencyInjection;

namespace Aida.Api.Extensions;

public static class ServiceRegister
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        return services.AddHealthFeature();
    }
}