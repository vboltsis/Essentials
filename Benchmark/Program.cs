using Benchmark;
using Benchmark.Classes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Numerics;

/*
 TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 
 */

//RUN MULTIPLE BENCHMARKS
//BenchmarkRunner.Run(new Type[] { typeof(Orderer), typeof(VectorsSum) });

//BenchmarkRunner.Run<Enumerables>();
//BenchmarkRunner.Run<EnumerablesClassVsStruct>();
//BenchmarkRunner.Run<SpanVsSubstring>();
//BenchmarkRunner.Run<ClassVsTuple>();
//BenchmarkRunner.Run<TaskVsValueTask>();
//BenchmarkRunner.Run<CachedTaskBenchmark>();
//BenchmarkRunner.Run<StringVsStringBuilder>();
//BenchmarkRunner.Run<VectorsSum>();
//BenchmarkRunner.Run<InitialCapacityClass>();
//BenchmarkRunner.Run<CheckIfNumberIsOdd>();
//BenchmarkRunner.Run<IndexOfVsContains>();
//BenchmarkRunner.Run<CheckIfNumberIsOdd>();
//BenchmarkRunner.Run<StringConverter>();
//BenchmarkRunner.Run<Orderer>();
BenchmarkRunner.Run<ContainsSetVsListVsArray>();


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

//var selections = new List<SelectionWithScore>
//    {
//        new SelectionWithScore
//        {
//            Home = 0,
//            Away = 0,
//            Selection = new Selection
//            {
//                Name = "0-0",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 0,
//            Away = 1,
//            Selection = new Selection
//            {
//                Name = "0-1",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 1,
//            Away = 1,
//            Selection = new Selection
//            {
//                Name = "1-1",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 0,
//            Away = 2,
//            Selection = new Selection
//            {
//                Name = "0-2",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 2,
//            Away = 2,
//            Selection = new Selection
//            {
//                Name = "2-2",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 1,
//            Away = 0,
//            Selection = new Selection
//            {
//                Name = "1-0",
//            }
//        }, 
//        new SelectionWithScore
//        {
//            Home = 2,
//            Away = 1,
//            Selection = new Selection
//            {
//                Name = "2-1",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 1,
//            Away = 2,
//            Selection = new Selection
//            {
//                Name = "1-2",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 2,
//            Away = 0,
//            Selection = new Selection
//            {
//                Name = "2-0",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 2,
//            Away = 3,
//            Selection = new Selection
//            {
//                Name = "2-3",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 3,
//            Away = 0,
//            Selection = new Selection
//            {
//                Name = "3-0",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 3,
//            Away = 1,
//            Selection = new Selection
//            {
//                Name = "3-1",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 0,
//            Away = 3,
//            Selection = new Selection
//            {
//                Name = "0-3",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 1,
//            Away = 3,
//            Selection = new Selection
//            {
//                Name = "1-3",
//            }
//        },
//        new SelectionWithScore
//        {
//            Home = 3,
//            Away = 2,
//            Selection = new Selection
//            {
//                Name = "3-2",
//            }
//        }
//    };

//var orderedSelections = new List<Selection>();
//orderedSelections.AddRange(selections
//    .Where(sel => sel.Home > sel.Away)
//    .OrderBy(sel => sel.Home)
//    .ThenBy(sel => sel.Away)
//    .Select(sel => sel.Selection));

//orderedSelections.AddRange(selections
//    .Where(sel => sel.Home == sel.Away)
//    .OrderBy(sel => sel.Home)
//    .Select(sel => sel.Selection));

//orderedSelections.AddRange(selections
//    .Where(sel => sel.Home < sel.Away)
//    .OrderBy(sel => sel.Away)
//    .ThenBy(sel => sel.Home)
//    .Select(sel => sel.Selection));

//var what = selections
//    .Select(s => s.Away < s.Home ?  (s.Home *10 + s.Away,  s.Selection) 
//    : s.Away == s.Home ? (100 + s.Home * 10 + s.Away, s.Selection) 
//    : (200 + s.Away *10 + s.Home, s.Selection))
//            .OrderBy(s => s.Item1)
//            .Select(s => s.Selection)
//            .ToList();
Console.WriteLine();
