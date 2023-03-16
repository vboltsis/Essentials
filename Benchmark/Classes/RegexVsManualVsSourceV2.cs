using System.Text.RegularExpressions;

namespace Benchmark.Classes;

/*
|              Method |               name |      Mean |    Error |   StdDev | Ratio | Rank |   Gen0 | Allocated | Alloc Ratio |
|-------------------- |------------------- |----------:|---------:|---------:|------:|-----:|-------:|----------:|------------:|
| TrimEncodeUrlNameV3 |     America(Clubs) |  31.92 ns | 0.666 ns | 0.556 ns |  0.11 |    1 | 0.0083 |     104 B |        1.00 |
| TrimEncodeUrlNameV2 |     America(Clubs) |  35.08 ns | 0.745 ns | 0.765 ns |  0.12 |    2 | 0.0083 |     104 B |        1.00 |
| TrimEncodeUrlNameV1 |     America(Clubs) | 298.17 ns | 5.108 ns | 4.778 ns |  1.00 |    3 | 0.0081 |     104 B |        1.00 |
|                     |                    |           |          |          |       |      |        |           |             |
| TrimEncodeUrlNameV3 | Canoe/Kayak Sprint |  37.30 ns | 0.311 ns | 0.345 ns |  0.16 |    1 | 0.0102 |     128 B |        2.00 |
| TrimEncodeUrlNameV2 | Canoe/Kayak Sprint |  40.63 ns | 0.402 ns | 0.336 ns |  0.17 |    2 | 0.0102 |     128 B |        2.00 |
| TrimEncodeUrlNameV1 | Canoe/Kayak Sprint | 238.39 ns | 4.168 ns | 3.898 ns |  1.00 |    3 | 0.0048 |      64 B |        1.00 |
|                     |                    |           |          |          |       |      |        |           |             |
| TrimEncodeUrlNameV3 |             U.S.A. |  21.24 ns | 0.446 ns | 0.654 ns |  0.07 |    1 | 0.0057 |      72 B |        1.00 |
| TrimEncodeUrlNameV2 |             U.S.A. |  24.64 ns | 0.423 ns | 0.375 ns |  0.08 |    2 | 0.0057 |      72 B |        1.00 |
| TrimEncodeUrlNameV1 |             U.S.A. | 322.52 ns | 6.205 ns | 6.640 ns |  1.00 |    3 | 0.0057 |      72 B |        1.00 |
 */

[RankColumn]
[MemoryDiagnoser]
public class RegexVsManualVsSourceV2
{
    [Params("America(Clubs)", "U.S.A.", "Canoe/Kayak Sprint")]
    public string name;

    public static readonly Regex UrlEncoder = new Regex(@"[^a-z\d]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
        var trimmed = UrlEncoder.Replace(name.Trim(), "-");
        trimmed = Regex.Replace(trimmed, "-$", string.Empty);
        return trimmed;
    }
}
