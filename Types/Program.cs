using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Adds console logging
    builder.SetMinimumLevel(LogLevel.Information); // Set minimum logging level
});

// Create a logger instance
ILogger<LoggingExample> logger = loggerFactory.CreateLogger<LoggingExample>();

var logging = new LoggingExample(logger);

logging.Example();