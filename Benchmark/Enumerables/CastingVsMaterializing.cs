namespace Benchmark;

[MemoryDiagnoser]
public class CastingVsMaterializing
{
    private static IEnumerable<int> _list = new int[] { 1, 2, 3, 4, 5 };

    [Benchmark(Baseline = true)]
    public int Casting()
    {
        var array = _list as int[];
        var sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }

    [Benchmark]
    public int Materializing()
    {
        var array = _list.ToArray();
        var sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }
}
