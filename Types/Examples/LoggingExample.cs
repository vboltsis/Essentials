using Microsoft.Extensions.Logging;

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(EventId = EventIdConstants.InfoUserLogin, Level = LogLevel.Information, Message = "User {UserId} has logged in.")]
    public static partial void LogUserLogin(this ILogger logger, int userId);

    [LoggerMessage(EventId = EventIdConstants.ErrorUserLogin, Level = LogLevel.Error, Message = "Error occurred while processing request.")]
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
        _logger.LogUserLogin(1);
        _logger.LogError();
    }
}
public static class EventIdConstants
{
    public const int InfoUserLogin = 1001;
    public const int ErrorUserLogin = 1002;
}