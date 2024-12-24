namespace Benchmark;

/*
| Method                             | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|----------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| SingleRandomInstance               | 29.219 us | 0.1209 us | 0.1071 us | 0.5493 |      - |   4.43 KB |
| ThreadLocalRandomInstances         | 75.436 us | 0.6302 us | 0.5895 us | 9.1553 | 0.1221 |  74.76 KB |
| RandomSharedInstance               |  9.683 us | 0.0238 us | 0.0223 us | 0.5493 |      - |   4.46 KB |
 */

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class RandomShared
{
    private static readonly Random singleRandom = new();
    private const int NumberOfIterations = 1000;

    [Benchmark]
    public void SingleRandomInstance()
    {
        Parallel.For(0, NumberOfIterations, _ =>
        {
            int value = singleRandom.Next();
        });
    }

    [Benchmark]
    public void ThreadLocalRandomInstances()
    {
        Parallel.For(0, NumberOfIterations, _ =>
        {
            int value = new Random().Next();
        });
    }

    [Benchmark]
    public void RandomSharedInstance()
    {
        Parallel.For(0, NumberOfIterations, _ =>
        {
            int value = Random.Shared.Next();
        });
    }
}
