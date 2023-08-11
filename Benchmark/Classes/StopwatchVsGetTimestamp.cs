using System.Diagnostics;

namespace Benchmark.Classes;

/*
|         Method |     Mean |   Error |  StdDev | Allocated |
|--------------- |---------:|--------:|--------:|----------:|
| StopwatchStart | 228.5 us | 0.41 us | 0.37 us |      40 B |
|   GetTimestamp | 228.6 us | 0.63 us | 0.49 us |         - |
*/

[MemoryDiagnoser]
public class StopwatchVsGetTimestamp
{
    private const int Million = 1_000_000;

    [Benchmark]
    public double StopwatchStart()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var sum = 0;
        for (int i = 0; i < Million; i++)
        {
            sum += i;
        }

        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMilliseconds;
    }

    [Benchmark]
    public double GetTimestamp()
    {
        var start = Stopwatch.GetTimestamp();

        var sum = 0;
        for (int i = 0; i < Million; i++)
        {
            sum += i;
        }

        var stop = Stopwatch.GetElapsedTime(start);

        return stop.TotalMilliseconds;
    }
}
