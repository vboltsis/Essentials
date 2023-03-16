namespace Benchmark.Classes;

/*
|                  Method | Number |       Mean |     Error |    StdDev | Allocated |
|------------------------ |------- |-----------:|----------:|----------:|----------:|
|  IndexOfCaseInsensitive |   1000 |  14.282 us | 0.0180 us | 0.0168 us |      48 B |
| ContainsCaseInsensitive |   1000 |  14.789 us | 0.0370 us | 0.0346 us |      48 B |
|    IndexOfCaseSensitive |   1000 |  23.140 us | 0.0683 us | 0.0639 us |      48 B |
|   ContainsCaseSensitive |   1000 |   8.674 us | 0.0301 us | 0.0282 us |      48 B |
|  IndexOfCaseInsensitive |  10000 | 175.355 us | 3.4725 us | 4.2645 us |      48 B |
| ContainsCaseInsensitive |  10000 | 167.159 us | 0.3320 us | 0.2943 us |      48 B |
|    IndexOfCaseSensitive |  10000 | 248.042 us | 1.1537 us | 1.0791 us |      48 B |
|   ContainsCaseSensitive |  10000 | 100.897 us | 0.1521 us | 0.1188 us |      48 B |
 */

[MemoryDiagnoser]
public class IndexOfVsContainsString {

    [Params(1000, 10_000)]
    public int Number;

    [GlobalSetup]
    public void Setup() {
        text = new string[Number];
        var random = new Random(100);

        for (int i = 0; i < text.Length; i++) {
            if (i % random.Next(2, 4) == 0) {
                text[i] = "test" + i;
            }
            else {
                text[i] = "TEST" + i;
            }
        }
    }

    private string[] text;

    [Benchmark]
    public int IndexOfCaseInsensitive() {
        return text.Where(static x => x.IndexOf("test9", StringComparison.OrdinalIgnoreCase) != -1).Count();
    }

    [Benchmark]
    public int ContainsCaseInsensitive() {
        return text.Where(static x => x.Contains("test9", StringComparison.OrdinalIgnoreCase)).Count();
    }

    [Benchmark]
    public int IndexOfCaseSensitive() {
        return text.Where(static x => x.IndexOf("test9") != -1).Count();
    }

    [Benchmark]
    public int ContainsCaseSensitive() {
        return text.Where(static x => x.Contains("test9")).Count();
    }
}
