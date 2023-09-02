﻿using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Minever.LowLevel.Tests.Integration;

public class XunitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;

    public XunitLoggerProvider(ITestOutputHelper output)
    {
        _output = output;
    }

    public ILogger CreateLogger(string categoryName) => new XunitLogger(_output);

    public void Dispose()
    {
    }
}
