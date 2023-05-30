namespace Minever.LowLevel.IntegrationTests;

public abstract class TestsBase
{
    protected virtual TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(1);
}
