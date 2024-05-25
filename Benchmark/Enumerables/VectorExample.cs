using System.Numerics;

namespace Benchmark;

/*
| Method       | Mean     | Error     | StdDev    | Allocated |
|------------- |---------:|----------:|----------:|----------:|
| StandardLoop | 8.600 us | 0.0433 us | 0.0383 us |         - |
| SimdVector   | 2.138 us | 0.0045 us | 0.0042 us |         - | 
*/

[MemoryDiagnoser]
public class VectorBenchmark
{
    private int[] array1;
    private int[] array2;
    private int[] result;

    [GlobalSetup]
    public void Setup()
    {
        array1 = new int[10000];
        array2 = new int[10000];
        result = new int[10000];
        var rand = new Random(2);
        for (int i = 0; i < array1.Length; i++)
        {
            array1[i] = rand.Next(100);
            array2[i] = rand.Next(100);
        }
    }

    [Benchmark]
    public int StandardLoop()
    {
        for (int i = 0; i < array1.Length; i++)
        {
            result[i] = array1[i] + array2[i];
        }

        return result.Sum();
    }

    [Benchmark]
    public int SimdVector()
    {
        int length = array1.Length;
        int vectorSize = Vector<int>.Count;
        int i;
        for (i = 0; i <= length - vectorSize; i += vectorSize)
        {
            var v1 = new Vector<int>(array1, i);
            var v2 = new Vector<int>(array2, i);
            (v1 + v2).CopyTo(result, i);
        }

        for (; i < length; i++)
        {
            result[i] = array1[i] + array2[i];
        }

        return result.Sum();
    }
}
