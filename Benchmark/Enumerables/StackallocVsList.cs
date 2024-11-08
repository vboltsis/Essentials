namespace Benchmark;

/*
| Method     | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------- |----------:|---------:|---------:|-------:|----------:|
| Stackalloc |  28.24 ns | 0.160 ns | 0.142 ns |      - |         - |
| List       | 109.26 ns | 1.618 ns | 1.435 ns | 0.0545 |     456 B | 
*/

[MemoryDiagnoser]
public class StackallocVsList
{
    [Benchmark]
    public void Stackalloc()
    {
        Span<int> span = stackalloc int[100];
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = i;
        }
    }

    [Benchmark]
    public void List()
    {
        List<int> list = new(100);
        for (int i = 0; i < list.Capacity; i++)
        {
            list.Add(i);
        }
    }
}
