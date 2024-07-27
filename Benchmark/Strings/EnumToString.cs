namespace Benchmark;

/*
| Method                        | Mean      | Error     | StdDev    | Gen0   | Allocated |
|------------------------------ |----------:|----------:|----------:|-------:|----------:|
| EnumToStringUsingToString     | 7.1722 ns | 0.1831 ns | 0.1880 ns | 0.0029 |      24 B |
| EnumToStringUsingEnumToString | 0.2602 ns | 0.0520 ns | 0.0557 ns |      - |         - |
*/

[MemoryDiagnoser]
public class EnumToString
{
    private Enumerate _enumerate = Enumerate.Fifth;

    [Benchmark]
    public string EnumToStringUsingToString()
    {
        return _enumerate.ToString();
    }

    [Benchmark]
    public string EnumToStringUsingEnumToString()
    {
        return _enumerate.EnumerateToString();
    }
}

public enum Enumerate
{
    First,
    Second,
    Third,
    Fourth,
    Fifth,
    Sixth,
    Seventh,
    Eighth,
    Ninth,
    Tenth
}

public static class EnumerateExtensions
{
    public static string EnumerateToString(this Enumerate enumerate)
    {
        return enumerate switch
        {
            Enumerate.First => nameof(Enumerate.First),
            Enumerate.Second => nameof(Enumerate.Second),
            Enumerate.Third => nameof(Enumerate.Third),
            Enumerate.Fourth => nameof(Enumerate.Fourth),
            Enumerate.Fifth => nameof(Enumerate.Fifth),
            Enumerate.Sixth => nameof(Enumerate.Sixth),
            Enumerate.Seventh => nameof(Enumerate.Seventh),
            Enumerate.Eighth => nameof(Enumerate.Eighth),
            Enumerate.Ninth => nameof(Enumerate.Ninth),
            Enumerate.Tenth => nameof(Enumerate.Tenth),
            _ => throw new ArgumentOutOfRangeException(nameof(enumerate), enumerate, null)
        };
    }
}
