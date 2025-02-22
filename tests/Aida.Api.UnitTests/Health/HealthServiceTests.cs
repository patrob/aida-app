using Aida.Api.Health;
using Aida.Api.Testing;
using FluentAssertions;

namespace Aida.Api.UnitTests.Health;

public class HealthServiceTests : BaseTest
{
    private readonly IHealthService _healthService = new HealthService();

    [Fact]
    public async Task IsHealthy_ShouldReturnTrue()
    {
        var result = await _healthService.IsHealthy();
        result.Should().BeTrue();
    }
}