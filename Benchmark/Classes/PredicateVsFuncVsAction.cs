namespace Benchmark;

/*
|             Method |      Mean |     Error |    StdDev | Rank | Allocated |
|------------------- |----------:|----------:|----------:|-----:|----------:|
|      FuncBenchmark | 0.4660 ns | 0.0140 ns | 0.0109 ns |    2 |         - |
| PredicateBenchmark | 0.4009 ns | 0.0224 ns | 0.0199 ns |    1 |         - |
|    ActionBenchmark | 0.4696 ns | 0.0258 ns | 0.0229 ns |    2 |         - |
*/

[RankColumn]
[MemoryDiagnoser] 
public class PredicateVsFuncVsAction
{
    private Func<int, int, int> divideFunc = (a, b) => a % b;
    private Predicate<int> isEvenPredicate = number => number % 2 == 0;
    private Action<int> doNothingAction = x => 
    {
        var y = x % 2;
    };

    [Benchmark]
    public int FuncBenchmark() => divideFunc(4, 2);

    [Benchmark]
    public bool PredicateBenchmark() => isEvenPredicate(4);

    [Benchmark]
    public void ActionBenchmark() => doNothingAction(4);
}
