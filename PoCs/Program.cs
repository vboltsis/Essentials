using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PoCs.BitWise;
using PoCs.Diffing;

BitWiseOperations.TestFruits();


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