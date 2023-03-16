using Benchmark.Classes;
using BenchmarkDotNet.Running;

//TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 

//RUN MULTIPLE BENCHMARKS
//BenchmarkRunner.Run(new Type[] { typeof(Orderer), typeof(VectorsSum) });

//SINGLE BENCHMARK RUN
//BenchmarkRunner.Run<Enumerables>();

//RUN THIS IN RELEASE MODE AND SELECT THE BENCHMARK FROM THE LIST
BenchmarkSwitcher.FromAssembly(typeof(Orderer).Assembly).Run();
