using InterviewExample;
using System.Text.Json;

//CHECK IF SUM PAIR EXISTS

//var array = new int[] { 1, 4, 45, 6, 10, 8 };
//int number = 17;
//Console.WriteLine(CheckIfSumPairExists.PairExists(array, number));

//CALCULATE PYTHAGORAS THEOREM
//Console.WriteLine(PythagorasTheorem.GetHypotenuse(3, 4));

//DAY OF THE WEEK

//Console.WriteLine(DayOfTheWeek.GetDayOfTheWeek("Tue", -11));
//Console.WriteLine(DayOfTheWeek.GetDayOfTheWeek("Mon", 10));

//EXCEPTION HANDLING

//try
//{
//    ExceptionHandling.Method2();
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.StackTrace.ToString());
//    Console.ReadKey();
//}

//FIBONACCI
//var noRecursion = Fibonacci.FibonacciNoRecursion(7);
//var recursion = Fibonacci.FibonacciRecursion(7);
//Console.WriteLine(noRecursion);
//Console.WriteLine(recursion);

//Given a string, print out the number of occurences of each letter in the string
var dictionary = new Dictionary<char, int>();
var text = "banana";

for (char c = 'z'; c >= 'a'; c--)
{
    dictionary.Add(c, 0);
}

foreach (var c in text)
{
    dictionary.TryGetValue(c, out var value);
    dictionary[c] = value + 1;
}

Console.WriteLine(JsonSerializer.Serialize(dictionary.ToList()));