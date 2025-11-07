using System.Text;

namespace Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class StringVsStringBuilder2
{
    [Params(10, 100)]
    public int Repetitions;

    [Benchmark(Baseline = true)]
    public string PlusEquals()
    {
        string s = string.Empty;
        for (int i = 0; i < Repetitions; i++)
            s += i.ToString();
        return s;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Repetitions; i++)
            sb.Append(i);
        return sb.ToString();
    }
}