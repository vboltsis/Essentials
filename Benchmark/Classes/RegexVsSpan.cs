using System.Text.RegularExpressions;

namespace Benchmark;

/*
|      Method |                 hash |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------ |--------------------- |----------:|----------:|----------:|-------:|----------:|
|   ParseHash | 13ogn(...)9|#42 [40] | 352.12 ns |  6.695 ns |  5.591 ns | 0.0806 |    1016 B |
| ParseHashV2 | 13ogn(...)9|#42 [40] |  76.15 ns |  1.236 ns |  1.032 ns | 0.0267 |     336 B |

|   ParseHash | aBc12(...)23456 [42] | 816.72 ns | 12.801 ns | 11.348 ns | 0.1583 |    1992 B |
| ParseHashV2 | aBc12(...)23456 [42] | 215.92 ns |  3.904 ns |  4.937 ns | 0.0653 |     824 B |
 */

[MemoryDiagnoser]
public class RegexVsSpan
{
    private static readonly Regex _hashReaderRegex = new Regex(@"(?<hash>[a-zA-Z0-9\+\/\=]+)\.(?<timestampSeconds>\d+)\|(?<randomValues>[\d\:\.\,]*)\#(?<id>[\d]+)", RegexOptions.Compiled);
    private static readonly char[] characters = new char[] { '.', '|', '#' };

    [Params("13ogntMvb33Qk6brT679HVw==.1685605589|#42", "aBc123+/=.1593847226|12:3.45,6:7.89#123456")]
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
        var enhancedOdds = new Dictionary<string, decimal?>();

        if (string.IsNullOrWhiteSpace(hash))
        {
            return null;
        }

        var parts = hash.Split(characters);

        if (parts.Length < 4)
        {
            return null;
        }

        if (!long.TryParse(parts[1], out var timestamp) || !int.TryParse(parts[^1], out var customerId))
        {
            return null;
        }

        var enhancedOddsString = parts[2];

        if (!string.IsNullOrWhiteSpace(enhancedOddsString))
        {
            var enhancedOddsParts = enhancedOddsString.Split(',');

            enhancedOdds = new Dictionary<string, decimal?>(enhancedOddsParts.Length);

            foreach (var part in enhancedOddsParts)
            {
                var subParts = part.Split(':');

                if (subParts.Length != 2 || !long.TryParse(subParts[0], out _) || !decimal.TryParse(subParts[1], out var value))
                {
                    throw new Exception("This shouldn't be happening");
                }

                enhancedOdds[subParts[0]] = value;
            }
        }

        return new SomeClass(parts[0], timestamp, enhancedOdds, customerId);
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
}