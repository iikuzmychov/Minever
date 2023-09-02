using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Minever.LowLevel.Tests.Integration;
public class XunitLogger : ILogger
{
    private readonly ITestOutputHelper _output;

    public XunitLogger(ITestOutputHelper output)
    {
        _output = output;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        var logLevelString = logLevel switch
        {
            LogLevel.Critical    => "CRIT",
            LogLevel.Error       => "EROR",
            LogLevel.Warning     => "WARN",
            LogLevel.Information => "INFO",
            LogLevel.Debug       => "DEBG",
            LogLevel.Trace       => "TRCE",
            
            _ => throw new NotSupportedException()
        };

        _output.WriteLine($"[{logLevelString}] {message}");
    }

    IDisposable? ILogger.BeginScope<TState>(TState state) => null!;
}
