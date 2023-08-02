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