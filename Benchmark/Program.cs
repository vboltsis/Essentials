using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Running;
using System;
using System.Numerics;

/*
 TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 
 */

//BenchmarkRunner.Run<Enumerables>();
//BenchmarkRunner.Run<EnumerablesClassVsStruct>();
//BenchmarkRunner.Run<SpanVsSubstring>();
//BenchmarkRunner.Run<ClassVsTuple>();
//BenchmarkRunner.Run<TaskVsValueTask>();
//BenchmarkRunner.Run<CachedTaskBenchmark>();
//BenchmarkRunner.Run<StringVsStringBuilder>();
//BenchmarkRunner.Run<VectorsSum>();
BenchmarkRunner.Run<IndexOfVsContains>();

//var dto1 = new RoleDto { Id = 1 };
//var dto2 = new RoleDto { Id = 1 };

//var list = new List<RoleDto> { dto1 };

//Console.WriteLine(list.Contains(dto2));
//Console.WriteLine(list.Any(d => d == dto2));
//Console.WriteLine(dto1 == dto2);

//public class RoleDto
//{
//    public int Id { get; set; }

//    public override bool Equals(object obj)
//    {
//        var o = obj as RoleDto;
//        return o.Id == Id;
//    }
//}

