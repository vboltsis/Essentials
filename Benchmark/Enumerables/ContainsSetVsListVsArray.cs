namespace Benchmark.Classes;
/*
|        Method | Number |       Mean |     Error |    StdDev |     Median | Allocated |
|-------------- |------- |-----------:|----------:|----------:|-----------:|----------:|
|   ContainsSet |     10 |   1.968 ns | 0.0337 ns | 0.0388 ns |   1.964 ns |         - |
|  ContainsList |     10 |   3.943 ns | 0.0363 ns | 0.0340 ns |   3.948 ns |         - |
| ContainsArray |     10 |   4.802 ns | 0.0446 ns | 0.0348 ns |   4.796 ns |         - |

|   ContainsSet |    100 |   1.957 ns | 0.0197 ns | 0.0175 ns |   1.958 ns |         - |
|  ContainsList |    100 |   6.149 ns | 0.1086 ns | 0.1016 ns |   6.104 ns |         - |
| ContainsArray |    100 |   6.989 ns | 0.0693 ns | 0.0648 ns |   6.989 ns |         - |

|   ContainsSet |   1000 |   1.960 ns | 0.0239 ns | 0.0224 ns |   1.962 ns |         - |
|  ContainsList |   1000 |  35.446 ns | 1.0005 ns | 2.9499 ns |  36.405 ns |         - |
| ContainsArray |   1000 |  36.994 ns | 0.5227 ns | 0.4634 ns |  36.978 ns |         - |

|   ContainsSet |  10000 |   2.044 ns | 0.0651 ns | 0.0975 ns |   1.992 ns |         - |
|  ContainsList |  10000 | 273.910 ns | 1.8131 ns | 1.6073 ns | 273.193 ns |         - |
| ContainsArray |  10000 | 247.535 ns | 4.2564 ns | 5.6822 ns | 244.387 ns |         - |
 */

[MemoryDiagnoser]
public class ContainsSetVsListVsArray
{
    private HashSet<int> _set;
    private List<int> _list;
    private int[] _array;

    [Params(10, 100, 1000, 10000)]
    public int Number;

    public int ContainedNumber => Number / 2;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _set = new HashSet<int>(Enumerable.Range(1, Number));
        _list = new List<int>(Enumerable.Range(1, Number));
        _array = Enumerable.Range(1, Number).ToArray();
    }

    [Benchmark]
    public bool ContainsSet()
    {
        return _set.Contains(ContainedNumber);
    }
    
    [Benchmark]
    public bool ContainsList()
    {
        return _list.Contains(ContainedNumber);
    }    
    
    [Benchmark]
    public bool ContainsArray()
    {
        return _array.Contains(ContainedNumber);
    }
}
