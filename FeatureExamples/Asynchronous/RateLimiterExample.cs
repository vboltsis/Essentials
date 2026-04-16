using System.Threading.RateLimiting;

namespace FeatureExamples;

/// <summary>
/// Shows how to apply the System.Threading.RateLimiting primitives shipped with .NET 7+.
/// </summary>
public static class RateLimiterExample
{
    public static async Task RunAsync()
    {
        await using var limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = 2,
            QueueLimit = 4,
            Window = TimeSpan.FromSeconds(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });

        var tasks = Enumerable.Range(0, 6).Select(id => SendAsync(id, limiter)).ToArray();

        await Task.WhenAll(tasks);
    }

    private static async Task SendAsync(int id, RateLimiter limiter)
    {
        using var lease = await limiter.AcquireAsync(1);

        if (lease.IsAcquired)
        {
            Console.WriteLine($"Request {id} acquired at {DateTime.UtcNow:O}");
            await Task.Delay(200); // Simulate work while holding the permit.
            return;
        }

        Console.WriteLine($"Request {id} was rejected because the limiter is at capacity.");
    }
}

