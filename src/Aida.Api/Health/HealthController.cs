using Microsoft.AspNetCore.Mvc;

namespace Aida.Api.Health;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public void Get()
    {
        // Healthy!
    }
}