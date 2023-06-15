namespace Minever.LowLevel.IntegrationTests;

public abstract class TestBase : IDisposable
{
    private readonly List<CancellationTokenSource> _cancellationTokenSources = new();

    protected virtual TimeSpan DefaultTimeout => TimeSpan.FromSeconds(1);

    public virtual void Dispose()
    {
        foreach (var source in _cancellationTokenSources)
        {
            source.Dispose();
        }
    }

    protected CancellationToken CreateDefaultTimeoutCancellationToken()
    {
        var cancellationTokenSource = new CancellationTokenSource(DefaultTimeout);
        _cancellationTokenSources.Add(cancellationTokenSource);

        return cancellationTokenSource.Token;
    }
}
