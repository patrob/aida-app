using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aida.Api.IntegrationTests;

public class CustomWebFactory : WebApplicationFactory<Program>
{
    // protected override void ConfigureWebHost(IWebHostBuilder builder)
    // {
    //     base.ConfigureWebHost(builder);
    // }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<CustomWebFactory>;