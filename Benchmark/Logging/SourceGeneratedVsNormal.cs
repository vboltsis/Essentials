using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Benchmark;

/*
| Method                                       | Mean     | Error    | StdDev   | Gen0   | Allocated |
|--------------------------------------------- |---------:|---------:|---------:|-------:|----------:|
| Log_WithoutIf_WithParams                     | 55.55 ns | 0.316 ns | 0.280 ns | 0.0105 |      88 B |
| Log_WithIf_WithParams                        | 56.74 ns | 0.437 ns | 0.409 ns | 0.0105 |      88 B |
| LogAdapter_WithoutIf_WithParams              | 61.47 ns | 0.448 ns | 0.397 ns | 0.0105 |      88 B |
| LoggerMessageDef_WithoutIf_WithParams        | 28.52 ns | 0.223 ns | 0.208 ns |      - |         - |
| LoggerMessage_SourceGen_WithoutIf_WithParams | 23.74 ns | 0.099 ns | 0.088 ns |      - |         - | 
*/

[MemoryDiagnoser]
public class SourceGeneratedVsNormal
{
    private const string LogMessageWithParameters =
        "This is a log message with parameters {First}, {Second}";

    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddFakeLogger().SetMinimumLevel(LogLevel.Information);
    });

    private readonly ILogger<SourceGeneratedVsNormal> _logger;
    private readonly ILoggerAdapter<SourceGeneratedVsNormal> _loggerAdapter;

    public SourceGeneratedVsNormal()
    {
        _logger = new Logger<SourceGeneratedVsNormal>(_loggerFactory);
        _loggerAdapter = new LoggerAdapter<SourceGeneratedVsNormal>(_logger);
    }

    [Benchmark]
    public void Log_WithoutIf_WithParams()
    {
        _logger.LogInformation(LogMessageWithParameters, 69, 420);
    }

    [Benchmark]
    public void Log_WithIf_WithParams()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(LogMessageWithParameters, 69, 420);
        }
    }

    [Benchmark]
    public void LogAdapter_WithoutIf_WithParams()
    {
        _loggerAdapter.LogInformation(LogMessageWithParameters, 69, 420);
    }

    [Benchmark]
    public void LoggerMessageDef_WithoutIf_WithParams()
    {
        _logger.LogBenchmarkMessage(69, 420);
    }

    [Benchmark]
    public void LoggerMessage_SourceGen_WithoutIf_WithParams()
    {
        _logger.LogBenchmarkMessageGen(69, 420);
    }

}

public static class LoggerMessageDefinitions
{
    private static readonly Action<ILogger, int, int, Exception?> BenchmarkedLogMessageDefinition =
        LoggerMessage.Define<int, int>(LogLevel.Information, 0,
            "This is a log message with parameters {First}, {Second}");

    public static void LogBenchmarkMessage(this ILogger logger, int first, int second)
    {
        BenchmarkedLogMessageDefinition(logger, first, second, null);
    }
}

public static partial class LoggerMessageDefinitionsGen
{
    [LoggerMessage(EventId = EventIdConstants.BenchmarkLogMessage, Level = LogLevel.Information,
        Message = "This is a log message with parameters {First}, {Second}",
        SkipEnabledCheck = true)]
    public static partial void LogBenchmarkMessageGen(this ILogger logger, int first, int second);
}

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message);
        }
    }

    public void LogInformation<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, arg0);
        }
    }

    public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, arg0, arg1);
        }
    }

    public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, arg0, arg1, arg2);
        }
    }
}

public interface ILoggerAdapter<T>
{
    void LogInformation(string message);

    void LogInformation<T0>(string message, T0 arg0);

    void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);

    void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
}

public class FakeLoggingProvider : ILoggerProvider
{
    public void Dispose()
    {

    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FakeLogger();
    }
}

public class FakeLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new FakeDisposable();
    }

    private class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

public static class FakeLoggerExtensions
{
    public static ILoggingBuilder AddFakeLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FakeLoggingProvider>());
        LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, FakeLoggingProvider>(builder.Services);
        return builder;
    }
}

public static class EventIdConstants
{
    public const int BenchmarkLogMessage = 0;
}

/* HOW TO TEST IN PROGRAM.CS
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Adds console logging
    builder.SetMinimumLevel(LogLevel.Information); // Set minimum logging level
});

// Create a logger instance
ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogBenchmarkMessageGen(1, 2);
*/