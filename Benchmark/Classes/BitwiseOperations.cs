namespace Benchmark;

[RankColumn]
[MemoryDiagnoser]
public class BitwiseOperations
{
    private static int _number = 123;

    [Benchmark]
    public bool IsEvenUsingModulo() => _number % 2 == 0;

    [Benchmark]
    public bool IsEvenUsingBitwise() => (_number & 1) == 0;
}
