using System.Threading.Tasks;

namespace Aida.Api.Health;

public interface IHealthService
{
    Task<bool> IsHealthy();
}

public class HealthService : IHealthService
{
    public async Task<bool> IsHealthy()
    {
        return await Task.FromResult(true);
    }
}