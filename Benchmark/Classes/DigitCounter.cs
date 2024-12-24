namespace Benchmark;

/*
| Method                | Mean     | Error     | StdDev    | Gen0   | Allocated |
|---------------------- |---------:|----------:|----------:|-------:|----------:|
| UsingArithmetic       | 5.437 ns | 0.0350 ns | 0.0273 ns |      - |         - |
| UsingStringConversion | 7.995 ns | 0.1054 ns | 0.0935 ns | 0.0048 |      40 B |
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class DigitCounter
{
    private int number = 123456789;

    [Benchmark]
    public int UsingArithmetic()
    {
        if (number == 0)
            return 1;

        int count = 0;
        int tempNumber = number;
        if (tempNumber < 0)
            tempNumber = -tempNumber;

        while (tempNumber > 0)
        {
            tempNumber /= 10;
            count++;
        }

        return count;
    }

    [Benchmark]
    public int UsingStringConversion()
    {
        return number.ToString().Length;
    }
}
