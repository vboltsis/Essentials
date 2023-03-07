namespace Benchmark.Classes;

/*
|   Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|--------- |---------:|--------:|--------:|-------:|----------:|
|  IndexOf | 310.3 ns | 5.44 ns | 5.09 ns | 0.0119 |     152 B |
| Contains | 138.9 ns | 0.99 ns | 0.77 ns | 0.0119 |     152 B |
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
