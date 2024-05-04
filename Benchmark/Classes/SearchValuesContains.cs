using System.Buffers;

namespace Benchmark;

/*
| Method               | Value                | Mean      | Error     | StdDev    | Allocated |
|--------------------- |--------------------- |----------:|----------:|----------:|----------:|
| IsBase64SearchValues | sDSFGDSFW            |  1.887 ns | 0.0600 ns | 0.0561 ns |         - |
| IsBase64Array        | sDSFGDSFW            | 19.154 ns | 0.0427 ns | 0.0356 ns |         - |
| IsBase64Loop         | sDSFGDSFW            | 29.725 ns | 0.1367 ns | 0.1067 ns |         - |
| IsBase64SearchValues | sDSFG(...)jUUi& [21] |  2.460 ns | 0.0168 ns | 0.0157 ns |         - |
| IsBase64Array        | sDSFG(...)jUUi& [21] | 56.959 ns | 0.3779 ns | 0.3535 ns |         - |
| IsBase64Loop         | sDSFG(...)jUUi& [21] | 35.028 ns | 0.2251 ns | 0.1879 ns |         - | 
*/

[MemoryDiagnoser]
public class SearchValuesContains
{
    private const string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    private static readonly char[] Base64CharactersArray = Base64Chars.ToCharArray();

    private static readonly SearchValues<char> searchValues = SearchValues.Create(Base64CharactersArray);

    private static readonly HashSet<char> Base64CharactersSet = new(Base64CharactersArray);

    [Params("sDSFGDSFW#dFGvbnjUUi&", "sDSFGDSFW")]
    public string Value { get; set; }

    [Benchmark]
    public bool IsBase64SearchValues()
    {
        return Value.AsSpan().IndexOfAnyExcept(searchValues) == -1;
    }

    [Benchmark]
    public bool IsBase64Array()
    {
        return Value.AsSpan().IndexOfAnyExcept(Base64CharactersArray) == -1;
    }

    [Benchmark]
    public bool IsBase64Loop()
    {
        foreach (var character in Value)
        {
            if (!Base64CharactersSet.Contains(character))
            {
                return false;
            }
        }

        return true;
    }

    //[Params("==sDSFGDSFW#dFGvbnjUUi&", "--sDSFGDSFW")]
    //public string Value2 { get; set; }

    //[Benchmark]
    //public bool ContainsSearchValues()
    //{
    //    return Value2.AsSpan().ContainsAny(searchValues);
    //}

    //[Benchmark]
    //public bool ContainsArray()
    //{
    //    return Value2.AsSpan().ContainsAny(Base64CharactersArray);
    //}
}

/*
| Method               | Value2               | Mean      | Error     | StdDev    | Allocated |
|--------------------- |--------------------- |----------:|----------:|----------:|----------:|
| ContainsSearchValues | --sDSFGDSFW          |  3.421 ns | 0.0813 ns | 0.0761 ns |         - |
| ContainsArray        | --sDSFGDSFW          |  8.827 ns | 0.1333 ns | 0.1181 ns |         - |
| ContainsSearchValues | ==sDS(...)jUUi& [23] |  3.740 ns | 0.0121 ns | 0.0107 ns |         - |
| ContainsArray        | ==sDS(...)jUUi& [23] | 53.416 ns | 0.3117 ns | 0.2763 ns |         - |
*/
