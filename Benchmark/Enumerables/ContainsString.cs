using System.Collections.Frozen;

namespace Benchmark;

/*
| Method            | Mean     | Error     | StdDev    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|
| ContainsHashSet   | 7.614 ns | 0.1223 ns | 0.1144 ns |         - |
| ContainsFrozenSet | 2.407 ns | 0.0223 ns | 0.0209 ns |         - |
| ContainsArray     | 9.547 ns | 0.1257 ns | 0.1176 ns |         - | 
*/

[MemoryDiagnoser]
public class ContainsString
{
    public static readonly HashSet<string> Statuses = 
        new HashSet<string>(4) { "all", "active", "unfulfilled", "completed" };

    public static readonly FrozenSet<string> Frozen = Statuses.ToFrozenSet();
    public static readonly string[] Array = ["all", "active", "unfulfilled", "completed"];

    [Benchmark]
    public bool ContainsHashSet()
    {
        return Statuses.Contains("active");
    }

    [Benchmark]
    public bool ContainsFrozenSet()
    {
        return Frozen.Contains("active");
    }

    [Benchmark]
    public bool ContainsArray()
    {
        return Array.Contains("active");
    }
}
