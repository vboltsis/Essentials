namespace Benchmark;

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class StringContainsCharacter
{
    [Params("This is Spart5", "22Tasrwuegfudvfjybv5jsdn", "e5rvvs")]
    public string Text { get; set; }

    [Benchmark]
    public bool Contains()
    {
        return Text.Contains("5");
    }

    [Benchmark]
    public bool ContainsCharacter()
    {
        return Text.Contains('5');
    }

    [Benchmark]
    public bool IndexOf()
    {
        return Text.IndexOf('5') != -1;
    }

    [Benchmark]
    public bool IndexOfSpan()
    {
        return Text.AsSpan().IndexOf('5') != -1;
    }

    [Benchmark]
    public bool SpanContainsCharacter()
    {
        return Text.AsSpan().Contains('5');
    }
}
