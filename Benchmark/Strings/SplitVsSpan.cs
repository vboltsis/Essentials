namespace Benchmark.Strings;

[MemoryDiagnoser]
public class SplitVsSpan
{
    [Params("RC_CatssonBaka_RanfarSelfEnlugion_PrCjumjer_12345678" )]
    public string Id { get; set; } = default!;

    [Benchmark(Baseline = true)]
    public string SplitAndLast_Linq()
        => Id.Split('_').Last();

    [Benchmark]
    public string LastIndexOf_Substring()
    {
        int idx = Id.LastIndexOf('_');
        return idx >= 0 ? Id.Substring(idx + 1) : Id;
    }

    [Benchmark]
    public string Span_LastIndexOf_ToString()
    {
        ReadOnlySpan<char> span = Id.AsSpan();
        int idx = span.LastIndexOf('_');

        return idx >= 0
            ? span[(idx + 1)..].ToString()
            : Id;
    }

    [Benchmark]
    public string Span_LastIndexOf_NewString_Slice()
    {
        ReadOnlySpan<char> span = Id.AsSpan();
        int idx = span.LastIndexOf('_');

        if (idx < 0)
            return Id;

        return new string(span.Slice(idx + 1));
    }
}