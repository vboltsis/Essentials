using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Running;

/*
 TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 
 */

//BenchmarkRunner.Run<Enumerables>();
//BenchmarkRunner.Run<EnumerablesClassVsStruct>();
//BenchmarkRunner.Run<SpanVsSubstring>();
BenchmarkRunner.Run<ClassVsTuple>();


/*
 
 public static class ToListExtension {

    public static List<T> ToList<T>(this IEnumerable<T> source, int capacity) 
    {
        var res = new List<T>(capacity);
        res.AddRange(source);
        return res;
    }

}
 */