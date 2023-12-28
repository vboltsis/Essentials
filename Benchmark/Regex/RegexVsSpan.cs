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

//var what = "13ogntMvb33Qk6brT679HVw==.1685605589|#42";
//var what = "aBc123+/=.1593847226|12:3.45,6:7.89#123456";

//var result = new RegexVsSpan().ParseHashV2(what);
//var result2 = new RegexVsSpan().ParseHash(what);

//Console.WriteLine(result.Id == result2.Id);
//Console.WriteLine(result.RandomValues == result2.RandomValues);
//Console.WriteLine(result.V == result2.V);
//Console.WriteLine(result.Timestamp == result2.Timestamp);

//string deserializedData = "{\"type\":1,\"target\":\"EventSyncStateData\",\"arguments\":[{\"pendingPenaltyTeam\":null,\"sportSettings\":{\"periods\":2,\"periodLength\":45,\"extraPeriods\":2,\"extraPeriodLength\":15,\"extraTimeTypeId\":1,\"penaltyFormatId\":1,\"awayGoalRuleId\":1,\"firstLegHomeScore\":null,\"firstLegAwayScore\":null,\"fastBetMarkets\":true,\"adjustPenaltySituation\":true},\"shootoutTotals\":null,\"rosterParams\":{\"home\":[],\"away\":[]},\"sourceMarkets\":{},\"periodBaseType\":1,\"nextPeriodTypeId\":1,\"nextPeriodTypeIds\":[1],\"incidents\":[],\"totals\":{\"1\":[0,0],\"20\":[0,0],\"31\":[0,0],\"30\":[0,0],\"40\":[0,0],\"80\":[0,0],\"50\":[0,0],\"60\":[0,0],\"61\":[0,0],\"70\":[0,0],\"90\":[0,0],\"81\":[0,0],\"83\":[0,0]},\"clock\":{\"seconds\":0,\"paused\":true},\"periods\":[],\"sport\":{\"playerBased\":false,\"id\":1,\"name\":\"Soccer\"},\"zone\":{\"id\":188567,\"name\":\"Europa League\"},\"league\":{\"id\":182761,\"name\":\"|Europa League| - |Matches|\"},\"eventTemplate\":{\"id\":10000642,\"name\":\"Bazooka\"},\"participants\":[{\"id\":108465,\"name\":\"Levadiakos\"},{\"id\":1986604,\"name\":\"Ionikos\"}],\"startTime\":1678895940000,\"endTime\":null,\"openTime\":null,\"closeTime\":null,\"autoSuspendNoIncident\":false,\"actualStartTime\":null,\"eventStatusId\":1,\"live\":false,\"started\":false,\"recovering\":false,\"eventFormatId\":1,\"resultsConfirmed\":false,\"deleted\":false,\"settled\":false,\"pricingModelPreEventId\":1,\"pricingModelInPlayId\":1,\"autoConfirmSecs\":290,\"autoSettleSecs\":null,\"clockModeId\":1,\"markets\":[],\"settings\":{\"1\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":true},\"7\":{\"active\":true,\"suspendedBy\":null,\"displayed\":true,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":true,\"inheritedProperties\":[\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"2\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"4\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"3\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"5\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"8\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"9\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"10\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"11\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"12\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"13\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"14\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"15\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"16\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"17\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"6\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false}},\"preEventOwner\":null,\"inPlayOwner\":null,\"panicButtons\":[{\"clicked\":false,\"clickedBySourceType\":null,\"id\":104,\"name\":\"WithdrawalFreeze\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":12,\"name\":\"All\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":14,\"name\":\"Penalty\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":15,\"name\":\"Scorers\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":8,\"name\":\"Booking\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":9,\"name\":\"Corner\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":10,\"name\":\"HalfTime\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":11,\"name\":\"Dangerous FK\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":58,\"name\":\"Stats\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":79,\"name\":\"Timed\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":87,\"name\":\"Draw\"}],\"navigationGroups\":[],\"notes\":[],\"channels\":null,\"hasBets\":false,\"specialPromotionState\":null,\"riskLevelId\":0,\"allowEarlyCashout\":true,\"allowIpPlayDelayOverride\":false,\"id\":14402664,\"name\":\"|Levadiakos| |vs| |Ionikos|\"}]}";

//var what = Encoding.UTF8.GetBytes(deserializedData);

//var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
//var compressedData = MessagePackSerializer.Serialize(what, lz4Options);

//var decompressedData = MessagePackSerializer.Deserialize<byte[]>(compressedData, lz4Options);

//Console.WriteLine();