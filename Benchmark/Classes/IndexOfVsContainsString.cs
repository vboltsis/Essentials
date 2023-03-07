namespace Benchmark.Classes;

/*
|   Method |     Mean |    Error |   StdDev |   Median |   Gen0 | Allocated |
|--------- |---------:|---------:|---------:|---------:|-------:|----------:|
|  IndexOf | 211.2 ns | 26.91 ns | 79.33 ns | 167.4 ns | 0.0038 |      48 B |
| Contains | 193.0 ns | 20.72 ns | 60.12 ns | 158.2 ns | 0.0038 |      48 B |
 */

[MemoryDiagnoser]
public class IndexOfVsContainsString {
    private static string[] text = new string[] {
        "test1",
        "test2",
        "test3",
        "test4",
        "test5",
        "testb6",
        "test7",
        "test8",
        "testb9",
        "test10",
        "testb11",
    };

    [Benchmark]
    public int IndexOf() {
        return text.Where(static x => x.IndexOf("testb", StringComparison.OrdinalIgnoreCase) != -1).Count();
    }

    [Benchmark]
    public int Contains() {
        return text.Where(static x => x.Contains("testb", StringComparison.OrdinalIgnoreCase)).Count();
    }
}
