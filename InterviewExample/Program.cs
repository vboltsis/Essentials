using System;
using InterviewExample;
using InterviewExample.Questions;

/*
UNCOMMENT THE PROBLEM YOU WANT TO DEBUG
*/

//CHECK IF SUM PAIR EXISTS

//var array = new int[] { 1, 4, 45, 6, 10, 8 };
//int number = 17;
//Console.WriteLine(CheckIfSumPairExists.PairExists(array, number));

//----------------------------------------------------------------------------

//CALCULATE PYTHAGORAS THEOREM
//Console.WriteLine(PythagorasTheorem.GetHypotenuse(3, 4));

//----------------------------------------------------------------------------

//DAY OF THE WEEK

//Console.WriteLine(DayOfTheWeek.GetDayOfTheWeek("Tue", -11));
//Console.WriteLine(DayOfTheWeek.GetDayOfTheWeek("Mon", 10));

//----------------------------------------------------------------------------

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

//----------------------------------------------------------------------------

//FIBONACCI

//var noRecursion = Fibonacci.FibonacciNoRecursion(7);
//var recursion = Fibonacci.FibonacciRecursion(7);
//Console.WriteLine(noRecursion);
//Console.WriteLine(recursion);

//----------------------------------------------------------------------------

//OCCURENCES OF LETTERS

//OccurencesOfLetters.GetOccurences("Banana");

//----------------------------------------------------------------------------

//COLLAZT CONJECTURE
var steps = CollatzConjecture.GetSteps(23443333456783233);
Console.WriteLine(steps);
