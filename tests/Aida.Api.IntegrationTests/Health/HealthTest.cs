namespace Aida.Api.IntegrationTests.Health;

[Collection("Integration")]
public class HealthTest(CustomWebFactory factory) : IClassFixture<CustomWebFactory>
{
    [Fact]
    public async Task Health_ShouldReturnSuccess()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
    }
}