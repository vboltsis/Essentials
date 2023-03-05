using Benchmark.Classes;
using BenchmarkDotNet.Running;
using MessagePack;
using MessagePack.Resolvers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

//TO USE THIS PROJECT, YOU MUST RUN IT IN RELEASE MODE 

//RUN MULTIPLE BENCHMARKS
//BenchmarkRunner.Run(new Type[] { typeof(Orderer), typeof(VectorsSum) });

//SINGLE BENCHMARK RUN
//BenchmarkRunner.Run<Enumerables>();

//RUN THIS IN RELEASE MODE AND SELECT THE BENCHMARK FROM THE LIST
BenchmarkSwitcher.FromAssembly(typeof(Orderer).Assembly).Run();

//var sampleData = new MyClass[100_000_000];
//for (int i = 0; i < sampleData.Length; i++) {
//    sampleData[i] = new MyClass {
//        MyBool = true,
//        MyDate = DateTime.Now.AddMinutes(1),
//        MyInt = i,
//        MyString = $"Hello World{i}"
//    };
//}
////write  the following 3 lines of code to execute in parallel for ever

//var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

//sampleData.AsParallel().ForAll(x => {

//    var data = MessagePackSerializer.Serialize(sampleData, options);

//    //get size of sampleData in MB
//    var size = data.Length / (1024m * 1024m);
//});

//var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

//var data = MessagePackSerializer.Serialize(sampleData, options);

////get size of sampleData in MB
//var size = data.Length / (1024m * 1024m);
//Console.WriteLine($"size of compressed: {size}MB");


//var k = 0;

//Regex _pipesMatchRegex = new Regex(@"\|([^\|]+)\|", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
////Regex _pipesMatchRegex = new Regex(@"\|(.*?)\|", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
//var wordsWithin = new List<string>();

//var what = "|makis test| omg vs |takis rest|";
//wordsWithin.AddRange(_pipesMatchRegex.Matches(what).Select(m => m.Value.Replace("|", "")));

//var translated = new List<string>
//{
//    "test 1",
//    "test 2"
//};

//var TranslationCodeDelimiterPattern = @"\|([^\|]+)\|";
//var translation = Regex.Replace(what, TranslationCodeDelimiterPattern, m => wordsWithin[k++]);
//var test = what;

//for (int i = 0; i < wordsWithin.Count; i++)
//{
//    var code = wordsWithin[i];
//    var tCode = translated[i];
//    test = test.Replace(code, tCode);
//}

//test = test.Replace("|", "");

//Console.WriteLine();


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
