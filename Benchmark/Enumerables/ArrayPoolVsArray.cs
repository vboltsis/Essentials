using System.Buffers;

namespace Benchmark;
/*
| Method              | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|-------------------- |----------:|----------:|----------:|----------:|-------:|----------:|
| Array               |  8.926 us | 0.1772 us | 0.4637 us |  8.689 us | 4.7607 |   40024 B |
| ArrayPool           | 12.721 us | 0.0565 us | 0.0501 us | 12.715 us |      - |         - |
| ArrayPoolNotCleared | 12.130 us | 0.0404 us | 0.0378 us | 12.145 us |      - |         - | 
*/

[MemoryDiagnoser]
public class ArrayPoolVsArray
{
    private const int ArraySize = 10_000;

    [Benchmark]
    public int Array()
    {
        int[] arr = new int[ArraySize];
        ComplexWork(arr);
        return 1;
    }

    [Benchmark]
    public int ArrayPool()
    {
        var pool = ArrayPool<int>.Shared;
        int[] rentedArray = pool.Rent(ArraySize);
        try
        {
            ComplexWork(rentedArray);
            return 1;
        }
        finally
        {
            pool.Return(rentedArray, clearArray: true);
        }
    }

    [Benchmark]
    public int ArrayPoolNotCleared()
    {
        var pool = ArrayPool<int>.Shared;
        int[] rentedArray = pool.Rent(ArraySize);
        try
        {
            ComplexWork(rentedArray);
            return 1;
        }
        finally
        {
            pool.Return(rentedArray, clearArray: false);
        }
    }

    private int ComplexWork(int[] array)
    {
        var number = 0;
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i % 256;
            number += (array[i] * i);
        }
        return number;
    }
}
