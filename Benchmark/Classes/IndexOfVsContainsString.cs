namespace Benchmark.Classes;

/*
|   Method |     Mean |    Error |   StdDev |   Median |   Gen0 | Allocated |
|--------- |---------:|---------:|---------:|---------:|-------:|----------:|
|  IndexOf | 211.2 ns | 26.91 ns | 79.33 ns | 167.4 ns | 0.0038 |      48 B |
| Contains | 193.0 ns | 20.72 ns | 60.12 ns | 158.2 ns | 0.0038 |      48 B |
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
