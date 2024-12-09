using Microsoft.Extensions.Logging;

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(EventId = EventIdConstants.InfoUserLogin, Level = LogLevel.Information, Message = "User {userId} has logged in {brandId} and jur {jurisdictionId}.", SkipEnabledCheck = true)]
    public static partial void LogUserLogin(this ILogger logger, Guid userId, int brandId, int jurisdictionId);

    [LoggerMessage(EventId = EventIdConstants.ErrorUserLogin, SkipEnabledCheck = true, Level = LogLevel.Error, Message = "Error occurred while processing request.")]
    public static partial void LogError(this ILogger logger);
}

public class LoggingExample
{
    private readonly ILogger<LoggingExample> _logger;

    public LoggingExample(ILogger<LoggingExample> logger)
    {
        _logger = logger;
    }

    public void Example()
    {
        _logger.LogUserLogin(Guid.NewGuid(),2, 420);
        _logger.LogError();
    }
}
public static class EventIdConstants
{
    public const int InfoUserLogin = 1001;
    public const int ErrorUserLogin = 1002;
}

/*
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Adds console logging
    builder.SetMinimumLevel(LogLevel.Information); // Set minimum logging level
});

// Create a logger instance
ILogger<LoggingExample> logger = loggerFactory.CreateLogger<LoggingExample>();

var logging = new LoggingExample(logger);

logging.Example();
*/