using System.Text;

namespace Benchmark;

/*
|                                    Method |       Mean |    Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|------------------------------------------ |-----------:|---------:|----------:|------:|-------:|----------:|------------:|
|             CreateStringWithConcatenation | 1,944.6 ns | 38.94 ns | 101.91 ns |  1.00 | 1.0014 |   12576 B |        1.00 |
|             CreateStringWithStringBuilder |   387.5 ns |  7.64 ns |  10.96 ns |  0.20 | 0.0610 |     768 B |        0.06 |
| CreateStringWithStringBuilderWithCapacity |   287.1 ns |  5.76 ns |  10.81 ns |  0.15 | 0.0391 |     496 B |        0.04 | 
*/

[RankColumn]
[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class StringCreation
{
    [Benchmark(Baseline = true)]
    public string CreateStringWithConcatenation()
    {
        string s = string.Empty;
        for (int i = 0; i < 100; i++)
        {
            s += 1.ToString();
        }

        return s;
    }

    [Benchmark]
    public string CreateStringWithStringBuilder()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 100; i++)
        {
            sb.Append(1);
        }

        return sb.ToString();
    }

    [Benchmark]
    public string CreateStringWithStringBuilderWithCapacity()
    {
        StringBuilder sb = new StringBuilder(100);
        for (int i = 0; i < 100; i++)
        {
            sb.Append(1);
        }

        return sb.ToString();
    }
}
