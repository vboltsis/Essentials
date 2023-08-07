using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PoCs.BitWise;
using PoCs.Diffing;

//VALUE TYPES
//int-datetime-decimal-long-short-bool-enum-struct-byte-sbyte-float-double-uint-ulong-ushort-char

//REFERENCE TYPES
//string-class-interface-object-dynamic


var user = new UserProfile 
{ 
    DateOfBirth = new DateTime(2000, 1, 1),
    Address = new Address { Street = "abc"} 
};

var dob = user.DateOfBirth;
dob = dob.AddMonths(1);  // This doesn't modify user.DateOfBirth because DateTime is immutable.
var address = user.Address;
address.Street = "def";  // This does modify user.Address because it's a reference type.

user.DateOfBirth = user.DateOfBirth.AddDays(10);  // This does modify user.DateOfBirth because it's a property setter.

var what = user.ToString();

Console.Read();
public record UserProfile
{
    public DateTime DateOfBirth { get; set; }
    public Address Address { get; set; }
}

public record Address
{
    public string Street { get; set; }
}
/*
         public T? Find(Predicate<T> match)
        {
            if (match == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }
 
            for (int i = 0; i < _size; i++)
            {
                if (match(_items[i]))
                {
                    return _items[i];
                }
            }
            return default;
        }
 */


////Date#Name#Age
//var dateText = "2023-01-01#Takis#25";
//var index = dateText.IndexOf('#');
//var date = DateTime.Parse(dateText.Substring(0, index));

//var span = dateText.AsSpan();
//var date2 = DateTime.Parse(span.Slice(0, index));


////BitWiseOperations.TestFruits();
//Regex _pipesMatchRegex = new Regex(@"\|([^\|]+)\|", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

//string testString = "This |test string| is designed to |demonstrate| how the |regex| works.|t|";
//string testString2 = "This |test string| is designed to |demonstrate| how the |regex works.";
//string testString3 = "This test string is designed to demonstrate how the |regex works.";
//string testString4 = "This test string is designed to demonstrate how the regex works.";


//var what = await TranslateRegex(testString);
//var result = await TranslateSubstrings(testString);

//Console.WriteLine(what == result);

//var result2 = ExtractSubstrings(testString2);
//var result3 = ExtractSubstrings(testString3);
//var result4 = ExtractSubstrings(testString4);

//Console.WriteLine();
//static List<string> ExtractSubstrings(string input)
//{
//    List<string> result = new List<string>();
//    ReadOnlySpan<char> span = input.AsSpan();

//    while (true)
//    {
//        int start = span.IndexOf('|');
//        if (start == -1)
//            break;
//        span = span.Slice(start + 1);
//        int end = span.IndexOf('|');
//        if (end == -1)
//            break;
//        result.Add(span.Slice(0, end).ToString());
//        span = span.Slice(end + 1);
//    }

//    return result;
//}
//async Task<string> TranslateRegex(string input)
//{
//    var matches = _pipesMatchRegex.Matches(input);
//    var translations = new List<string>(matches.Count);

//    foreach (var item in matches)
//    {
//        translations.Add("Takis");
//    }

//    var i = 0;
//    return Regex.Replace(input, _pipesMatchRegex.ToString(), m => translations[i++]);
//}

//static async Task<string> TranslateSubstrings(string input)
//{
//    var span = input.AsMemory();
//    var builder = new StringBuilder();

//    int previousEnd = 0;

//    while (true)
//    {
//        int start = span.Span.IndexOf('|');
//        if (start == -1)
//        {
//            builder.Append(span.ToString());
//            break;
//        }
//        builder.Append(span.Slice(0, start).ToString());
//        span = span.Slice(start + 1);
//        int end = span.Span.IndexOf('|');
//        if (end == -1)
//        {
//            builder.Append("|" + span.ToString());
//            break;
//        }
//        string toTranslate = span.Slice(0, end).ToString();
//        string translation = "Takis";
//        builder.Append(translation);
//        span = span.Slice(end + 1);
//    }

//    return builder.ToString();
//}

//var tasks = new List<Task>(5);

//for (int i = 0; i < 5; i++)
//{
//    //await ThrowMethod(i);
//    tasks.Add(ThrowMethod(i));
//}

//await Task.WhenAll(tasks);

//Console.WriteLine();

//async Task ThrowMethod(int i)
//{
//    try
//    {
//        Console.WriteLine(i);
//        await Task.Delay(1000);
//        throw new Exception("Exception");
//    }
//    catch (Exception)
//    {
//        Console.WriteLine($"Exception: {i}");
//    }
//}

/*
var what = TrimEncodeUrlName("Canoe/Kayak Sprint");
var what2 = TrimEncodeUrlName2("Canoe/Kayak Sprint");

Console.WriteLine(what);
Console.WriteLine(what2);

static string TrimEncodeUrlName(string name)
{
    if (string.IsNullOrEmpty(name))
    {
        return string.Empty;
    }

    name = name.Trim();
    var buffer = new char[name.Length];
    int bufferIndex = 0;
    bool previousWasInvalid = false;

    for (int i = 0; i < name.Length; i++)
    {
        char current = name[i];
        if (char.IsLetterOrDigit(current))
        {
            buffer[bufferIndex++] = current;
            previousWasInvalid = false;
        }
        else if (!previousWasInvalid)
        {
            buffer[bufferIndex++] = '-';
            previousWasInvalid = true;
        }
    }

    if (bufferIndex > 0 && buffer[bufferIndex - 1] == '-')
    {
        bufferIndex--;
    }

    var trimmed = new string(buffer.AsSpan(0, bufferIndex));
    return trimmed;
}

static string TrimEncodeUrlName2(string name)
{
    if (string.IsNullOrEmpty(name))
    {
        return string.Empty;
    }

    ReadOnlySpan<char> nameSpan = name.AsSpan();
    int startIndex = 0;
    int endIndex = nameSpan.Length - 1;

    while (startIndex < nameSpan.Length && char.IsWhiteSpace(nameSpan[startIndex]))
    {
        startIndex++;
    }

    while (endIndex >= startIndex && char.IsWhiteSpace(nameSpan[endIndex]))
    {
        endIndex--;
    }

    nameSpan = nameSpan.Slice(startIndex, endIndex - startIndex + 1);
    var buffer = new char[nameSpan.Length];
    int bufferIndex = 0;
    bool previousWasInvalid = false;

    for (int i = 0; i < nameSpan.Length; i++)
    {
        char current = nameSpan[i];
        if (char.IsLetterOrDigit(current))
        {
            buffer[bufferIndex++] = current;
            previousWasInvalid = false;
        }
        else if (!previousWasInvalid)
        {
            buffer[bufferIndex++] = '-';
            previousWasInvalid = true;
        }
    }

    if (bufferIndex > 0 && buffer[bufferIndex - 1] == '-')
    {
        bufferIndex--;
    }

    var trimmed = new string(buffer.AsSpan(0, bufferIndex));
    return trimmed;
}

//America(Clubs)-- > America - Clubs
// Canoe / Kayak Sprint-- > Canoe - Kayak - Sprint
//U.S.A.  ->U - S - A
 
 */


//ITest test1 = new Test
//{
//    Age = 1
//};

//ITest test2 = new Test
//{
//    Age = 1
//};

//Console.WriteLine(test2.Equals(test1));
//Console.WriteLine();

//class Test : ITest
//{
//    public int Age { get; set; }
//}

//interface ITest
//{
//    int Age { get; set; }

//    public bool Equals(ITest other)
//    {
//        if (other == null)
//            return false;
//        return other.Age == Age;
//    }
//}

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



//var test = new Test();
//test.Name = "test";
//test.Name = "test";

//decimal value = 4.70m;
//decimal value1 = 4.77m;
//decimal value2 = 4.00m;
////decimal value3 = 4.700m;
//decimal value3 = 4.7150m;

//decimal truncatedValue = (int)(value * 100) / 100m;

//Console.WriteLine(value.ToString("0.###"));
//Console.WriteLine(value1.ToString("0.##"));
//Console.WriteLine(value2.ToString("0.##"));
//Console.WriteLine(value3.ToString("0.##"));
//Console.WriteLine(truncatedValue);

//var list = new List<string>(2);
//list.Add("Takis");
//list.Add("Makis");
//list.Add("Sakis");
//Console.WriteLine(list.LastOrDefault());

//Test1.Texts1.Add(Test1.Text);
//Test1.Texts1.Add("sakis");
//Test1.Texts2.Add(Test1.Text); 
//Test1.Texts2.Add("takis"); 
//Console.WriteLine("");

//public class Test1
//{
//    public const string Text = "text";

//    public static HashSet<string> Texts1 = new HashSet<string>();
//    public static HashSet<string> Texts2 = new HashSet<string>();
//}



//var json = "{\"set\":[1]}";
//var json = @"{""name"": ""makis"", ""set"":[] }";

//var value = JsonConvert.DeserializeObject<MyModel>(json);
////var value = JsonSerializer.Deserialize<MyModel>(json);

//Console.WriteLine();

//public class MyModel
//{
//    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
//    public HashSet<int> Set { get; set; } = new HashSet<int> { 3, 4, 5 };

//    public string Name { get; set; }
//}

//class Test
//{
//    private string _name;

//    public string Name
//    {
//        get => _name;
//        set
//        {
//            Console.WriteLine("hello");
//            if (_name == value)
//            {
//                return;
//            }
//            _name = value;
//        }
//    }
//}