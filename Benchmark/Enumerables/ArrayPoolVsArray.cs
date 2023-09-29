using System.Buffers;

namespace Benchmark;
/*
|                 Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|----------------------- |---------:|--------:|--------:|-------:|----------:|
| RegularArrayAllocation | 593.8 ns | 6.04 ns | 5.65 ns | 0.4807 |    4024 B |
|         UsingArrayPool | 526.4 ns | 1.10 ns | 0.98 ns |      - |         - | 
*/

[MemoryDiagnoser]
public class ArrayPoolVsArray
{
    private const int ArraySize = 1_000;

    [Benchmark]
    public void RegularArrayAllocation()
    {
        int[] arr = new int[ArraySize];
        SimpleWork(arr);
    }

    [Benchmark]
    public void UsingArrayPool()
    {
        var pool = ArrayPool<int>.Shared;

        int[] rentedArray = pool.Rent(ArraySize);

        try
        {
            SimpleWork(rentedArray);
        }
        finally
        {
            pool.Return(rentedArray, clearArray: true);
        }
    }

    private int SimpleWork(int[] array)
    {
        var number = 0;

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
            number += array[i];
        }

        return number;
    }
}
