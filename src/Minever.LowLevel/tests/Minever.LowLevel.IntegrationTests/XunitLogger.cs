using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Minever.LowLevel.IntegrationTests;
public class XunitLogger : ILogger
{
    private readonly string _categoryName;
    private readonly ITestOutputHelper _output;

    public XunitLogger(string categoryName, ITestOutputHelper output)
    {
        _categoryName = categoryName;
        _output       = output;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => null!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        var logLevelString = logLevel switch
        {
            LogLevel.Critical     => "CRIT",
            LogLevel.Error        => "EROR",
            LogLevel.Warning      => "WARN",
            LogLevel.Information  => "INFO",
            LogLevel.Debug        => "DEBG",
            LogLevel.Trace        => "TRCE",
            
            _ => throw new NotSupportedException()
        };

        // todo: do we need category name ???
        //_output.WriteLine($"{_categoryName}:");
        _output.WriteLine($"[{logLevelString}] {message}");
        //_output.WriteLine(string.Empty);
    }
}
