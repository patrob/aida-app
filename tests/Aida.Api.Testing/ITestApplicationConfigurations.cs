namespace Aida.Api.Testing;

public interface ITestApplicationConfigurations
{
    bool UseFakeUsers { get; set; }
    bool UseFakeAuth { get; set; }
    bool UseFakeStripeServices { get; set; }
}

public class TestApplicationConfigurations : ITestApplicationConfigurations
{
    public bool UseFakeUsers { get; set; } = true;
    public bool UseFakeAuth { get; set; } = true;
    public bool UseFakeStripeServices { get; set; } = true;
} 