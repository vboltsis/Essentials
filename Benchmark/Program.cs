using BenchmarkDotNet.Running;

/*TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 

RUN MULTIPLE BENCHMARKS
BenchmarkRunner.Run(new Type[] { typeof(Orderer), typeof(VectorsSum) });

SINGLE BENCHMARK RUN
BenchmarkRunner.Run<Enumerables>();

RUN THIS IN RELEASE MODE AND SELECT THE BENCHMARK FROM THE LIST
BenchmarkSwitcher.FromAssembly(typeof(Orderer).Assembly).Run();

WILL PRINT THE BENCHMARKS IN ALPHABETICAL ORDER IN THE CONSOLE
*/

var types = typeof(Program).Assembly.GetTypes()
    .Where(t => t.GetCustomAttributes(typeof(MemoryDiagnoserAttribute), false).Length != 0)
    .OrderBy(t => t.Name)
    .ToArray();

BenchmarkSwitcher.FromTypes(types).Run();
