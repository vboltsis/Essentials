using System.Numerics;

namespace Benchmark.Classes;

/*
|  Method |     N |          Mean |      Error |     StdDev |        Median |     Ratio |  RatioSD | Allocated | Alloc Ratio |
|-------- |------ |--------------:|-----------:|-----------:|--------------:|----------:|---------:|----------:|------------:|
|     Sum |    10 |     0.2716 ns |  0.0310 ns |  0.0566 ns |     0.2452 ns |      1.00 |     0.00 |         - |          NA |
| SumLINQ |    10 |     5.2644 ns |  0.0266 ns |  0.0236 ns |     5.2578 ns |     18.73 |     4.00 |         - |          NA |
|  SumFor |    10 |     2.7648 ns |  0.0806 ns |  0.1207 ns |     2.7240 ns |     10.13 |     2.09 |         - |          NA |
|         |       |               |            |            |               |           |          |           |             |
|     Sum |   100 |     0.2239 ns |  0.0050 ns |  0.0044 ns |     0.2232 ns |      1.00 |     0.00 |         - |          NA |
| SumLINQ |   100 |    34.8732 ns |  0.7095 ns |  1.0619 ns |    34.4649 ns |    157.11 |     7.21 |         - |          NA |
|  SumFor |   100 |    29.9885 ns |  0.3387 ns |  0.3003 ns |    29.9776 ns |    133.98 |     3.76 |         - |          NA |
|         |       |               |            |            |               |           |          |           |             |
|     Sum |  1000 |     0.2373 ns |  0.0278 ns |  0.0342 ns |     0.2231 ns |      1.00 |     0.00 |         - |          NA |
| SumLINQ |  1000 |   280.7047 ns |  1.9021 ns |  1.7792 ns |   280.3107 ns |  1,170.07 |   172.55 |         - |          NA |
|  SumFor |  1000 |   252.6365 ns |  1.1686 ns |  0.9124 ns |   252.7006 ns |  1,060.85 |   149.28 |         - |          NA |
|         |       |               |            |            |               |           |          |           |             |
|     Sum | 10000 |     0.1379 ns |  0.0108 ns |  0.0084 ns |     0.1361 ns |      1.00 |     0.00 |         - |          NA |
| SumLINQ | 10000 | 2,750.6489 ns | 20.1754 ns | 18.8721 ns | 2,742.1165 ns | 20,046.19 | 1,235.61 |         - |          NA |
|  SumFor | 10000 | 2,499.4205 ns | 13.8628 ns | 12.9672 ns | 2,493.9159 ns | 18,183.27 | 1,074.83 |         - |          NA |
 */

[MemoryDiagnoser] 
public class VectorsSum
{
    [Params(10, 100, 1000, 10000)]
    public int N;
    private int[] _numbers { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _numbers = new int[N];
        for (int i = 0; i < N; i++)
        {
            _numbers[i] = i;
        }
    }

    [Benchmark(Baseline = true)]
    public int Sum()
    {
        var vector = new Vector<int>(_numbers);
        return Vector.Sum(vector);
    }

    [Benchmark]
    public int SumLINQ()
    {
        return _numbers.Sum();
    }

    [Benchmark]
    public int SumFor()
    {
        var sum = 0;
        for (int i = 0; i < N; i++)
        {
            sum += i;
        }

        return sum;
    }
}
