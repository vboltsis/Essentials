using Benchmark;
using BenchmarkDotNet.Running;

/*
 TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 
 */

//BenchmarkRunner.Run<Enumerables>();


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