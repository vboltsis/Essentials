using System.Text;

namespace Benchmark;

/*
    | Method          | Mean     | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------------- |---------:|--------:|--------:|------:|-------:|----------:|------------:|
| FormatString    | 147.2 ns | 2.16 ns | 1.69 ns |  1.00 | 0.0181 |     152 B |        1.00 |
| FormatComposite | 130.4 ns | 1.85 ns | 1.81 ns |  0.89 | 0.0124 |     104 B |        0.68 | 
*/

[MemoryDiagnoser]
public class CompositeFormatVsStringFormat
{
    private static readonly CompositeFormat s_format = CompositeFormat.Parse(SR.CurrentTime);

    [Benchmark(Baseline = true)]
    public string FormatString() => string.Format(null, SR.CurrentTime, DateTime.Now, 10);

    [Benchmark]
    public string FormatComposite() => string.Format(null, s_format, DateTime.Now, 10);
}

internal static class SR
{
    //suppose this was loaded from a resource file
    public static string CurrentTime => "The current time is {0:t}. My age is {1}";
}
