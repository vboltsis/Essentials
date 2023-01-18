using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PoCs.BitWise;
using PoCs.Diffing;


BitWiseOperations.TestFruits();

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