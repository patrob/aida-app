using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Aida.Api.Health;

[ApiController]
[Route("[controller]")]
public class HealthController(IHealthService healthService) : ControllerBase
{
    [HttpGet]
    public async Task<bool> Get()
    {
        return await healthService.IsHealthy();
    }
}