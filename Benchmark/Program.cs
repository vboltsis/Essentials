using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.Text;

/*TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 

RUN MULTIPLE BENCHMARKS
BenchmarkRunner.Run(new Type[] { typeof(Orderer), typeof(VectorsSum) });

SINGLE BENCHMARK RUN
BenchmarkRunner.Run<Enumerables>();

RUN THIS IN RELEASE MODE AND SELECT THE BENCHMARK FROM THE LIST
BenchmarkSwitcher.FromAssembly(typeof(Orderer).Assembly).Run();

WILL PRINT THE BENCHMARKS IN ALPHABETICAL ORDER IN THE CONSOLE
*/

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

var types = typeof(Program).Assembly.GetTypes()
    .Where(t => t.GetCustomAttributes(typeof(MemoryDiagnoserAttribute), false).Any())
    .OrderBy(t => t.Name)
    .ToArray();

BenchmarkSwitcher.FromTypes(types).Run();
