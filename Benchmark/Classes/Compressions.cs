﻿using MessagePack;
using System.IO.Compression;
using System.Text;

namespace Benchmark.Classes;

/*
|                      Method |      Mean |     Error |    StdDev | Rank | Code Size |   Gen0 |   Gen1 | Allocated |
|---------------------------- |----------:|----------:|----------:|-----:|----------:|-------:|-------:|----------:|
|         CompressMessagePack |  5.162 us | 0.0987 us | 0.0923 us |    1 |   2,711 B | 0.1602 |      - |   1.98 KB |
|       CompressBrotliOptimal | 71.055 us | 1.3917 us | 2.2867 us |    6 |   2,623 B | 1.4648 |      - |  19.29 KB |
|       CompressBrotliFastest | 19.843 us | 0.3592 us | 0.3360 us |    2 |   2,626 B | 1.8005 | 0.0916 |  22.43 KB |
|      CompressDeflateOptimal | 44.473 us | 0.8880 us | 2.0403 us |    4 |   3,651 B | 1.5869 | 0.0610 |  19.52 KB |
|      CompressDeflateFastest | 24.119 us | 0.2816 us | 0.2496 us |    3 |   3,654 B | 1.6174 | 0.0916 |  19.84 KB |
| CompressDeflateSmallestSize | 64.939 us | 0.4318 us | 0.3828 us |    5 |   3,654 B | 1.5869 |      - |  19.46 KB |

 */

[RankColumn]
[MemoryDiagnoser] 
//[DisassemblyDiagnoser]
public class Compressions
{
    //private byte[] data;
    private string deserializedData = "{\"type\":1,\"target\":\"EventSyncStateData\",\"arguments\":[{\"pendingPenaltyTeam\":null,\"sportSettings\":{\"periods\":2,\"periodLength\":45,\"extraPeriods\":2,\"extraPeriodLength\":15,\"extraTimeTypeId\":1,\"penaltyFormatId\":1,\"awayGoalRuleId\":1,\"firstLegHomeScore\":null,\"firstLegAwayScore\":null,\"fastBetMarkets\":true,\"adjustPenaltySituation\":true},\"shootoutTotals\":null,\"rosterParams\":{\"home\":[],\"away\":[]},\"sourceMarkets\":{},\"periodBaseType\":1,\"nextPeriodTypeId\":1,\"nextPeriodTypeIds\":[1],\"incidents\":[],\"totals\":{\"1\":[0,0],\"20\":[0,0],\"31\":[0,0],\"30\":[0,0],\"40\":[0,0],\"80\":[0,0],\"50\":[0,0],\"60\":[0,0],\"61\":[0,0],\"70\":[0,0],\"90\":[0,0],\"81\":[0,0],\"83\":[0,0]},\"clock\":{\"seconds\":0,\"paused\":true},\"periods\":[],\"sport\":{\"playerBased\":false,\"id\":1,\"name\":\"Soccer\"},\"zone\":{\"id\":188567,\"name\":\"Europa League\"},\"league\":{\"id\":182761,\"name\":\"|Europa League| - |Matches|\"},\"eventTemplate\":{\"id\":10000642,\"name\":\"Bazooka\"},\"participants\":[{\"id\":108465,\"name\":\"Levadiakos\"},{\"id\":1986604,\"name\":\"Ionikos\"}],\"startTime\":1678895940000,\"endTime\":null,\"openTime\":null,\"closeTime\":null,\"autoSuspendNoIncident\":false,\"actualStartTime\":null,\"eventStatusId\":1,\"live\":false,\"started\":false,\"recovering\":false,\"eventFormatId\":1,\"resultsConfirmed\":false,\"deleted\":false,\"settled\":false,\"pricingModelPreEventId\":1,\"pricingModelInPlayId\":1,\"autoConfirmSecs\":290,\"autoSettleSecs\":null,\"clockModeId\":1,\"markets\":[],\"settings\":{\"1\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":true},\"7\":{\"active\":true,\"suspendedBy\":null,\"displayed\":true,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":true,\"inheritedProperties\":[\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"2\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"4\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"3\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"5\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"8\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"9\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"10\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"11\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"12\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"13\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"14\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"15\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"16\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"17\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false},\"6\":{\"active\":true,\"suspendedBy\":null,\"displayed\":false,\"displayOrder\":0,\"inPlayDelaySecs\":7,\"allowCashoutPreEvent\":true,\"allowCashoutInPlay\":true,\"overaskPreEvent\":2000.0000,\"overaskInPlay\":4000.0000,\"stakeLimitPreEvent\":999999.00,\"stakeLimitInPlay\":999999.00,\"maxPayout\":50000.0000,\"allowMultiples\":true,\"availablePreEvent\":true,\"availableInPlay\":false,\"inheritedProperties\":[\"active\",\"displayed\",\"displayOrder\",\"inPlayDelaySecs\",\"overaskPreEvent\",\"overaskInPlay\",\"stakeLimitPreEvent\",\"stakeLimitInPlay\",\"maxPayout\",\"allowMultiples\",\"availablePreEvent\",\"availableInPlay\",\"allowCashoutInPlay\",\"allowCashoutPreEvent\",\"overaskScheduler\"],\"overaskScheduler\":{\"items\":[{\"id\":67391693,\"timePeriodEnum\":1,\"overaskPercentage\":100},{\"id\":67391694,\"timePeriodEnum\":2,\"overaskPercentage\":100},{\"id\":67391695,\"timePeriodEnum\":3,\"overaskPercentage\":100}]},\"hasOveraskRules\":false}},\"preEventOwner\":null,\"inPlayOwner\":null,\"panicButtons\":[{\"clicked\":false,\"clickedBySourceType\":null,\"id\":104,\"name\":\"WithdrawalFreeze\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":12,\"name\":\"All\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":14,\"name\":\"Penalty\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":15,\"name\":\"Scorers\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":8,\"name\":\"Booking\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":9,\"name\":\"Corner\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":10,\"name\":\"HalfTime\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":11,\"name\":\"Dangerous FK\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":58,\"name\":\"Stats\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":79,\"name\":\"Timed\"},{\"clicked\":false,\"clickedBySourceType\":null,\"id\":87,\"name\":\"Draw\"}],\"navigationGroups\":[],\"notes\":[],\"channels\":null,\"hasBets\":false,\"specialPromotionState\":null,\"riskLevelId\":0,\"allowEarlyCashout\":true,\"allowIpPlayDelayOverride\":false,\"id\":14402664,\"name\":\"|Levadiakos| |vs| |Ionikos|\"}]}";

    //[GlobalSetup]
    //public void GlobalSetup() {
    //    // Generate some sample data to compress
    //    var sampleData = new MyClass[100_000];
    //    for (int i = 0; i < sampleData.Length; i++) {
    //        sampleData[i] = new MyClass {
    //            MyBool = true,
    //            MyDate = DateTime.Now.AddMinutes(1),
    //            MyInt = i,
    //            MyString = $"Hello World{i}"
    //        };
    //    }

    //    deserializedData = sampleData;
    //    var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
    //    // Serialize the data to a byte array
    //    data = MessagePackSerializer.Serialize(sampleData, options);
    //}

    [Benchmark]
    public byte[] CompressMessagePack()
    {
        return MessagePackSerializer.Serialize(deserializedData,
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
    }

    [Benchmark]
    public byte[] CompressBrotliOptimal()
    {
        byte[] inputData = Encoding.UTF8.GetBytes(deserializedData);
        using var outputStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(outputStream, CompressionLevel.Optimal))
        {
            brotliStream.Write(inputData, 0, inputData.Length);
        }
        return outputStream.ToArray();
    }

    [Benchmark]
    public byte[] CompressBrotliFastest()
    {
        byte[] inputData = Encoding.UTF8.GetBytes(deserializedData);
        using var outputStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(outputStream, CompressionLevel.Fastest))
        {
            brotliStream.Write(inputData, 0, inputData.Length);
        }
        return outputStream.ToArray();
    }

    [Benchmark]
    public byte[] CompressDeflateOptimal()
    {
        byte[] inputData = Encoding.UTF8.GetBytes(deserializedData);
        using var outputStream = new MemoryStream();
        using (var deflateStream = new DeflateStream(outputStream, CompressionLevel.Optimal))
        {
            deflateStream.Write(inputData, 0, inputData.Length);
        }
        return outputStream.ToArray();
    }

    [Benchmark]
    public byte[] CompressDeflateFastest()
    {
        byte[] inputData = Encoding.UTF8.GetBytes(deserializedData);
        using var outputStream = new MemoryStream();
        using (var deflateStream = new DeflateStream(outputStream, CompressionLevel.Fastest))
        {
            deflateStream.Write(inputData, 0, inputData.Length);
        }
        return outputStream.ToArray();
    }

    [Benchmark]
    public byte[] CompressDeflateSmallestSize()
    {
        byte[] inputData = Encoding.UTF8.GetBytes(deserializedData);
        using var outputStream = new MemoryStream();
        using (var deflateStream = new DeflateStream(outputStream, CompressionLevel.SmallestSize))
        {
            deflateStream.Write(inputData, 0, inputData.Length);
        }
        return outputStream.ToArray();
    }

    //[Benchmark]
    //public MyClass[] Decompress() {
    //    return MessagePackSerializer.Deserialize<MyClass[]>(data,
    //        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
    //}
}

[MessagePackObject]
public class MyClass {

    [Key(0)]
    public int MyInt { get; set; }
    [Key(1)]
    public string MyString { get; set; }
    [Key(2)]
    public bool MyBool { get; set; }
    [Key(3)]
    public DateTime MyDate { get; set; }
}