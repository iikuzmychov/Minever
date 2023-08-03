using Microsoft.Extensions.Logging;
using Minever.LowLevel.Java.Core;
using Xunit.Abstractions;

namespace Minever.LowLevel.Tests.Integration;

public abstract class TestBase : IDisposable
{
    private readonly List<CancellationTokenSource> _cancellationTokenSources = new();

    protected ILogger<JavaProtocolClient> ClientLogger { get; }
    protected virtual TimeSpan DefaultTimeout => TimeSpan.FromSeconds(1);

    public TestBase(ITestOutputHelper output)
    {
        ClientLogger = LoggerFactory
            .Create(builder => builder
                .SetMinimumLevel(LogLevel.Trace)
                .AddProvider(new XunitLoggerProvider(output)))
            .CreateLogger<JavaProtocolClient>();
    }

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
