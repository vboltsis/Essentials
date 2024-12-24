namespace Benchmark;

/*
| Method            | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------ |---------:|---------:|---------:|-------:|----------:|
| UsingStringCreate | 34.18 ns | 0.652 ns | 0.578 ns | 0.0076 |      64 B |
| UsingStringConcat | 25.57 ns | 0.378 ns | 0.335 ns | 0.0076 |      64 B |
| UsingStringFormat | 55.15 ns | 0.640 ns | 0.598 ns | 0.0105 |      88 B |
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class StringBuilderVsCreateString
{
    private string name = "John";
    private int age = 30;

    [Benchmark]
    public string UsingStringCreate()
    {
        return string.Create(20, (name, age), (span, state) =>
        {
            var (name, age) = state;
            name.AsSpan().CopyTo(span);
            span[name.Length] = ' ';
            "is ".AsSpan().CopyTo(span.Slice(name.Length + 1));
            age.ToString().AsSpan().CopyTo(span.Slice(name.Length + 4));
            span[name.Length + 4 + age.ToString().Length] = ' ';
            "years old".AsSpan().CopyTo(span.Slice(name.Length + 5 + age.ToString().Length));
        });
    }

    [Benchmark]
    public string UsingStringConcat()
    {
        return string.Concat(name, " is ", age.ToString(), " years old");
    }

    [Benchmark]
    public string UsingStringFormat()
    {
        return string.Format("{0} is {1} years old", name, age);
    }
}
