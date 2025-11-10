namespace FeatureExamples;

/// <summary>
/// Demonstrates C# 12 primary constructors on classes by wiring dependencies without boilerplate.
/// </summary>
public class PrimaryConstructorWorker(string name, int maxBatchSize, TimeProvider timeProvider)
{
    public string Name { get; } = name;
    public int MaxBatchSize { get; } = maxBatchSize;

    private readonly TimeProvider _timeProvider = timeProvider;

    public string CreateMessage(ReadOnlySpan<string> items)
    {
        var timestamp = _timeProvider.GetLocalNow();
        return $"{Name} processed {items.Length} items at {timestamp:t} (max batch size {MaxBatchSize}).";
    }
}

public static class PrimaryConstructorExample
{
    public static void Run()
    {
        var worker = new PrimaryConstructorWorker("Telemetry", 10, TimeProvider.System);
        Console.WriteLine(worker.CreateMessage(["Request", "Cache", "Database"]));

        var deterministic = new DeterministicTimeProvider(new DateTimeOffset(2024, 11, 1, 12, 30, 0, TimeSpan.Zero));
        var playbackWorker = new PrimaryConstructorWorker("Playback", 3, deterministic);
        Console.WriteLine(playbackWorker.CreateMessage(["Replay"]));
    }
}

file sealed class DeterministicTimeProvider(DateTimeOffset now) : TimeProvider
{
    private DateTimeOffset _now = now;

    public override DateTimeOffset GetUtcNow() => _now;

    public override long GetTimestamp() => TimeProvider.System.GetTimestamp();

    public override long TimestampFrequency => TimeProvider.System.TimestampFrequency;
}

