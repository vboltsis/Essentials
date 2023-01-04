using System;
using PoCs.BitWise;
using PoCs.Diffing;

var test = new MyClass
{
    MyProperty = "test"
};


test.MyProperty = "test2";

Console.WriteLine();
//BitWiseOperations.TestFruits();

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