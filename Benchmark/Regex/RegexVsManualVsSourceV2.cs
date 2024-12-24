using System.Text.RegularExpressions;

namespace Benchmark;

/*
|              Method |               name |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
|-------------------- |------------------- |----------:|---------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| TrimEncodeUrlNameV3 |     America(Clubs) |  32.22 ns | 0.559 ns | 0.549 ns |  0.09 |    0.00 |    1 | 0.0083 |     104 B |        1.08 |
| TrimEncodeUrlNameV2 |     America(Clubs) |  35.56 ns | 0.463 ns | 0.387 ns |  0.10 |    0.00 |    2 | 0.0083 |     104 B |        1.08 |
| TrimEncodeUrlNameV1 |     America(Clubs) | 349.38 ns | 6.140 ns | 5.128 ns |  1.00 |    0.00 |    4 | 0.0076 |      96 B |        1.00 |
| TrimEncodeUrlNameV0 |     America(Clubs) | 298.29 ns | 2.507 ns | 2.094 ns |  0.85 |    0.01 |    3 | 0.0081 |     104 B |        1.08 |
|                     |                    |           |          |          |       |         |      |        |           |             |
| TrimEncodeUrlNameV3 | Canoe/Kayak Sprint |  37.43 ns | 0.731 ns | 0.684 ns |  0.13 |    0.00 |    1 | 0.0102 |     128 B |        2.29 |
| TrimEncodeUrlNameV2 | Canoe/Kayak Sprint |  41.58 ns | 0.835 ns | 0.928 ns |  0.15 |    0.01 |    2 | 0.0102 |     128 B |        2.29 |
| TrimEncodeUrlNameV1 | Canoe/Kayak Sprint | 277.12 ns | 5.494 ns | 7.701 ns |  1.00 |    0.00 |    4 | 0.0043 |      56 B |        1.00 |
| TrimEncodeUrlNameV0 | Canoe/Kayak Sprint | 237.06 ns | 4.349 ns | 4.068 ns |  0.85 |    0.02 |    3 | 0.0048 |      64 B |        1.14 |
|                     |                    |           |          |          |       |         |      |        |           |             |
| TrimEncodeUrlNameV3 |             U.S.A. |  21.21 ns | 0.355 ns | 0.296 ns |  0.09 |    0.00 |    1 | 0.0057 |      72 B |        3.00 |
| TrimEncodeUrlNameV2 |             U.S.A. |  24.83 ns | 0.497 ns | 0.816 ns |  0.11 |    0.00 |    2 | 0.0057 |      72 B |        3.00 |
| TrimEncodeUrlNameV1 |             U.S.A. | 225.26 ns | 3.323 ns | 2.946 ns |  1.00 |    0.00 |    3 | 0.0019 |      24 B |        1.00 |
| TrimEncodeUrlNameV0 |             U.S.A. | 329.48 ns | 6.604 ns | 8.816 ns |  1.48 |    0.05 |    4 | 0.0057 |      72 B |        3.00 |
 */

[RankColumn]
[MemoryDiagnoser] 
public partial class RegexVsManualVsSourceV2
{
    [Params("America(Clubs)", "U.S.A.", "Canoe/Kayak Sprint")]
    public string name;

    public static readonly Regex UrlEncoder = new Regex(@"[^a-z\d]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    [GeneratedRegex(@"[^a-z\d]+")]
    private static partial Regex IsAlphanumericRegex();

    [Benchmark]
    public string TrimEncodeUrlNameV3()
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }

        ReadOnlySpan<char> nameSpan = name.AsSpan();
        int startIndex = 0;
        int endIndex = nameSpan.Length - 1;

        while (startIndex < nameSpan.Length && char.IsWhiteSpace(nameSpan[startIndex]))
        {
            startIndex++;
        }

        while (endIndex >= startIndex && char.IsWhiteSpace(nameSpan[endIndex]))
        {
            endIndex--;
        }

        nameSpan = nameSpan.Slice(startIndex, endIndex - startIndex + 1);
        var buffer = new char[nameSpan.Length];
        int bufferIndex = 0;
        bool previousWasInvalid = false;

        for (int i = 0; i < nameSpan.Length; i++)
        {
            char current = nameSpan[i];
            if (char.IsLetterOrDigit(current))
            {
                buffer[bufferIndex++] = current;
                previousWasInvalid = false;
            }
            else if (!previousWasInvalid)
            {
                buffer[bufferIndex++] = '-';
                previousWasInvalid = true;
            }
        }

        if (bufferIndex > 0 && buffer[bufferIndex - 1] == '-')
        {
            bufferIndex--;
        }

        var trimmed = new string(buffer.AsSpan(0, bufferIndex));
        return trimmed;
    }

    [Benchmark]
    public string TrimEncodeUrlNameV2()
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }

        name = name.Trim();
        var buffer = new char[name.Length];
        int bufferIndex = 0;
        bool previousWasInvalid = false;

        for (int i = 0; i < name.Length; i++)
        {
            char current = name[i];
            if (char.IsLetterOrDigit(current))
            {
                buffer[bufferIndex++] = current;
                previousWasInvalid = false;
            }
            else if (!previousWasInvalid)
            {
                buffer[bufferIndex++] = '-';
                previousWasInvalid = true;
            }
        }

        if (bufferIndex > 0 && buffer[bufferIndex - 1] == '-')
        {
            bufferIndex--;
        }

        var trimmed = new string(buffer.AsSpan(0, bufferIndex));
        return trimmed;
    }

    [Benchmark(Baseline = true)]
    public string TrimEncodeUrlNameV1()
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }
        var trimmed = IsAlphanumericRegex().Replace(name.Trim(), "-");
        trimmed = Regex.Replace(trimmed, "-$", string.Empty);
        return trimmed;
    }

    [Benchmark]
    public string TrimEncodeUrlNameV0()
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }
        var trimmed = UrlEncoder.Replace(name.Trim(), "-");
        trimmed = Regex.Replace(trimmed, "-$", string.Empty);
        return trimmed;
    }
}
