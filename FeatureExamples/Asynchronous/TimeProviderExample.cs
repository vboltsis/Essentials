namespace FeatureExamples;

/// <summary>
/// Demonstrates the .NET 8 TimeProvider integration with PeriodicTimer for testable scheduling.
/// </summary>
public static class TimeProviderExample
{
    public static async Task RunAsync()
    {
        var provider = new StepTimeProvider(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(500));

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1), provider);

        for (var i = 0; i < 3; i++)
        {
            await timer.WaitForNextTickAsync();
            Console.WriteLine($"Tick {i + 1} observed at {provider.LastProvidedUtc:O}");
        }
    }
}

file sealed class StepTimeProvider(DateTimeOffset start, TimeSpan step) : TimeProvider
{
    private DateTimeOffset _current = start;
    private DateTimeOffset _lastProvidedUtc = start;

    public override DateTimeOffset GetUtcNow()
    {
        var now = _current;
        _current = _current.Add(step);
        _lastProvidedUtc = now;
        return now;
    }

    public override long GetTimestamp() => TimeProvider.System.GetTimestamp();

    public override long TimestampFrequency => TimeProvider.System.TimestampFrequency;

    public DateTimeOffset LastProvidedUtc => _lastProvidedUtc;
}

