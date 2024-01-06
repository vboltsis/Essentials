using System.Globalization;
using System.Text.RegularExpressions;

namespace Benchmark;

/*
| Method      | hash                 | Mean        | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------ |--------------------- |------------:|----------:|----------:|-------:|-------:|----------:|
| ParseHash   | 1jgK5(...)93#84 [55] |   609.58 ns |  4.298 ns |  4.020 ns | 0.1402 |      - |    1760 B |
| ParseHashV2 | 1jgK5(...)93#84 [55] |   129.46 ns |  1.175 ns |  1.042 ns | 0.0408 |      - |     512 B |

| ParseHash   | 1xZ3b(...)|#38! [41] |   296.52 ns |  3.880 ns |  3.630 ns | 0.0806 |      - |    1016 B |
| ParseHashV2 | 1xZ3b(...)|#38! [41] |    46.01 ns |  0.528 ns |  0.494 ns | 0.0159 |      - |     200 B |

| ParseHash   | 1Y54(...)2#58 [174]  | 1,920.08 ns | 14.230 ns | 12.615 ns | 0.4387 | 0.0038 |    5504 B |
| ParseHashV2 | 1Y54(...)2#58 [174]  |   593.65 ns |  4.036 ns |  3.775 ns | 0.0992 |      - |    1248 B |
 */

[MemoryDiagnoser]
public class RegexVsSpan
{
    private static readonly Regex _hashReaderRegex = new Regex(@"(?<hash>[a-zA-Z0-9\+\/\=]+)\.(?<timestampSeconds>\d+)\|(?<randomValues>[\d\:\.\,]*)\#(?<id>[\d]+)", RegexOptions.Compiled);
    private static char colon = ':';

    [Params("1xZ3bUYdCDzq40efL08KS2A==.1702048130|#38!", "1jgK5Ls/0kukZlQj9GH5qsw==.1702282106|2902143786:1.93#84", "1Y547weMkZtRmuMDcKTHNwQ==.1703237432|4324725873:2.62,4324733733:3.3,4324881336:4,4324890378:4.35,4324892133:3.4,4324914088:4.1,4324922484:3.15,4324946134:4,4324950622:2.82#58")]
    public string hash { get; set; }

    [Benchmark]
    public SomeClass ParseHash()
    {
        if (string.IsNullOrWhiteSpace(hash) ||
            !(_hashReaderRegex.Match(hash) is { Success: true } match) ||
            !long.TryParse(match.Groups["timestampSeconds"].Value, out var timestamp) ||
            !int.TryParse(match.Groups["id"].Value, out var Id))
        {
            return null;
        }

        var enhancedOddsString = match.Groups["randomValues"].Value;

        var enhancedOdds = string.IsNullOrWhiteSpace(enhancedOddsString)
            ? new Dictionary<string, decimal?>()
            : enhancedOddsString.Split(',').Select(x => x.Split(':'))
                .GroupBy(x => long.TryParse(x[0], out _) ? x[0] : throw new Exception("This shouldn't be happening"))
                .ToDictionary(x => x.Key, x => (decimal?)decimal.Parse(x.First()[1]));

        return new SomeClass(match.Groups["hash"].Value, timestamp, enhancedOdds, Id);
    }

    [Benchmark]
    public SomeClass ParseHashV2()
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return null;
        }

        ReadOnlySpan<char> randomString = string.Empty;
        var hashSpan = hash.AsSpan();

        var dotIndex = hashSpan.IndexOf('.');

        if (dotIndex is -1)
        {
            return null;
        }

        var text = hashSpan[..dotIndex].ToString();

        var pipeIndex = hashSpan.IndexOf('|');

        if (pipeIndex is -1)
        {
            return null;
        }

        ReadOnlySpan<char> timestampSpan = hashSpan[(dotIndex + 1)..pipeIndex];

        if (!long.TryParse(timestampSpan, out long timestamp))
        {
            return null;
        }

        var dotsIndex = hashSpan.IndexOf(':');
        var hashIndex = hashSpan.IndexOf('#');

        var randomValues = new Dictionary<string, decimal?>();
        if (dotsIndex != -1)
        {
            randomString = hashSpan[(pipeIndex + 1)..hashIndex];
        }

        ReadOnlySpan<char> customer = hashSpan[(hashIndex + 1)..];

        var customerId = ExtractIntegerPart(customer);

        if (customerId is -1 || timestamp is 0 || string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        if (!randomString.IsEmpty)
        {
            randomValues = ParseKeyValuePairs(randomString);
        }

        return new SomeClass(text, timestamp, randomValues, customerId);
    }

    static int ExtractIntegerPart(ReadOnlySpan<char> inputSpan)
    {
        int startIndex = -1;
        int length = 0;

        for (int i = 0; i < inputSpan.Length; i++)
        {
            if (char.IsDigit(inputSpan[i]))
            {
                if (startIndex == -1)
                {
                    startIndex = i;
                }
                length++;
            }
            else if (startIndex != -1)
            {
                break;
            }
        }

        return startIndex != -1 ? int.Parse(inputSpan.Slice(startIndex, length)) : -1;
    }

    static Dictionary<string, decimal?> ParseKeyValuePairs(ReadOnlySpan<char> span)
    {
        var colonOccurrences = GetOccurrencesOfCharacter(span);
        var result = new Dictionary<string, decimal?>(colonOccurrences);

        while (true)
        {
            int commaIndex = span.IndexOf(',');
            var pair = commaIndex >= 0 ? span[..commaIndex] : span;

            int colonIndex = pair.IndexOf(':');
            if (colonIndex < 0)
                break;

            var keySpan = pair[..colonIndex];
            var valueSpan = pair[(colonIndex + 1)..];

            var key = new string(keySpan);
            if (decimal.TryParse(valueSpan, out decimal value))
            {
                result.Add(key, value);
            }

            if (commaIndex < 0)
                break;

            span = span[(commaIndex + 1)..];
        }

        return result;
    }

    static int GetOccurrencesOfCharacter(ReadOnlySpan<char> span)
    {
        var count = 0;
        for (var i = span.Length - 1; i >= 0; i--)
        {
            if (span[i] == colon)
            {
                count++;
            }
        }

        return count;
    }
}

public record SomeClass
{
    public int Id { get; set; }
    public string V { get; set; }
    public long Timestamp { get; set; }
    public Dictionary<string, decimal?> RandomValues { get; set; }
    public SomeClass(string v, long timestamp, Dictionary<string, decimal?> randomDouble, int id)
    {
        V = v;
        Timestamp = timestamp;
        RandomValues = randomDouble;
        Id = id;
    }

    public static void CompareNewAndOldParsing(SomeClass old,
        SomeClass newHash)
    {
        if (newHash.V == old.V &&
            newHash.Id == old.Id &&
            newHash.Timestamp == old.Timestamp &&
            newHash.RandomValues?.Count == old.RandomValues?.Count)
        {
            if (old.RandomValues?.Count > 0)
            {
                if (newHash.RandomValues?.Count > 0)
                {
                    if (newHash.RandomValues.Values.Intersect(old.RandomValues.Values).Count() !=
                        old.RandomValues.Values.Count)
                    {
                        Console.WriteLine("New and old hash parsing are different for hash");
                    }
                }

                if (!new HashSet<string>(newHash.RandomValues.Keys)
                    .SetEquals(old.RandomValues.Keys))
                {
                    Console.WriteLine("New and old hash parsing are different for hash");
                }
            }
        }
        else
        {
            Console.WriteLine("New and old hash parsing are different for hash");
        }
    }
}

/*
var text = "1jgK5Ls/0kukZlQj9GH5qsw==.1702282106|2902143786:1.93#84";
//var text = "1Y547weMkZtRmuMDcKTHNwQ==.1703237432|4324725873:2.62,4324733733:3.3,4324881336:4,4324890378:4.35,4324892133:3.4,4324914088:4.1,4324922484:3.15,4324946134:4,4324950622:2.82#58";
//var text = "1xZ3bUYdCDzq40efL08KS2A==.1702048130|#38!";
var regexVsSpan = new RegexVsSpan
{
    hash = text
};

var result = regexVsSpan.ParseHash();
var result2 = regexVsSpan.ParseHashV2();

var what = result2.RandomValues.Values.Intersect(result.RandomValues.Values).ToList();
SomeClass.CompareNewAndOldParsing(result, result);
*/