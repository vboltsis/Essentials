using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MessagePack;
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

namespace Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        var config = ManualConfig.CreateMinimumViable();
        config.ArtifactsPath = "/app/benchmarks";
        BenchmarkRunner.Run<ChannelsUnbounded>(config);
        //BenchmarkRunner.Run<ChannelsUnbounded>();

        //var types = typeof(Program).Assembly.GetTypes()
        //    .Where(t => t.GetCustomAttributes(typeof(MemoryDiagnoserAttribute), false).Any())
        //    .OrderBy(t => t.Name)
        //    .ToArray();

        //BenchmarkSwitcher.FromTypes(types).Run();
    }
}
