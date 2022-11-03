using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Running;

/*
 TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 
 */

//BenchmarkRunner.Run<Enumerables>();
//BenchmarkRunner.Run<EnumerablesClassVsStruct>();
//BenchmarkRunner.Run<SpanVsSubstring>();
//BenchmarkRunner.Run<ClassVsTuple>();
//BenchmarkRunner.Run<TaskVsValueTask>();
//BenchmarkRunner.Run<CachedTaskBenchmark>();
BenchmarkRunner.Run<StringVsStringBuilder>();

