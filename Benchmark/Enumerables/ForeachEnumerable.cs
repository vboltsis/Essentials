namespace Benchmark;

/*
| Method              | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|-------------------- |----------:|----------:|----------:|----------:|-------:|----------:|
| Foreach             |  6.189 ns | 0.2488 ns | 0.7336 ns |  5.854 ns |      - |         - |
| For                 |  4.958 ns | 0.0882 ns | 0.0825 ns |  4.973 ns |      - |         - |
| ForeachOnEnumerable | 20.292 ns | 0.4241 ns | 0.3967 ns | 20.252 ns | 0.0048 |      40 B | 
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class ForeachEnumerable
{
    private static List<int> list = Enumerable.Range(0, 10).ToList();

    [Benchmark]
    public int Foreach()
    {
        int sum = 0;
        foreach (int item in list)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int For()
    {
        int sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            sum += list[i];
        }
        return sum;
    }

    [Benchmark]
    public int ForeachOnEnumerable()
    {
        int sum = 0;
        IEnumerable<int> enumerable = list as IEnumerable<int>;
        foreach (int item in enumerable)
        {
            sum += item;
        }
        return sum;
    }
}
