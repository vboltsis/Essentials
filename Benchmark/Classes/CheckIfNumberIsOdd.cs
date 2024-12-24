namespace Benchmark.Classes;

[MemoryDiagnoser] 
public class CheckIfNumberIsOdd
{
    [Params(100_000, 1_000_000)]
    public int _number;
    private int[] _numbers { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _numbers = Enumerable.Range(0, _number).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int SumOdd()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] % 2 != 0)
                sum += _numbers[i];
        }

        return sum;
    }

    [Benchmark]
    public int SumOddBit()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            if ((_numbers[i] & 1) == 0)
                sum += _numbers[i];
        }

        return sum;
    }

    [Benchmark]
    public int SumOddNoBranch()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            var element = _numbers[i];
            var odd = element & 1;
            sum += odd * element;
        }

        return sum;
    }
}
