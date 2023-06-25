using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Minever.LowLevel.IntegrationTests;

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;

    public XunitLoggerProvider(ITestOutputHelper output)
    {
        _output = output;
    }

    public ILogger CreateLogger(string categoryName) => new XunitLogger(categoryName, _output);

    public void Dispose()
    {
    }
}
