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

//OccurrencesOfLetters.GetOccurrences("Banana");

//----------------------------------------------------------------------------

//COLLAZT CONJECTURE
//var steps = CollatzConjecture.GetSteps(23443333456783233);
//Console.WriteLine(steps);

//----------------------------------------------------------------------------

//SORT COMMA SEPARATED INTEGERS
//var numbers = "343,535,449,352,536,493,350,352,355,625,582,627,583,351,436,437,439,643,460,644,441,440,442,631,2797,2798,2799,4072,4073,4074,4075,4120,4068,4069,4070,4071,4119,4064,4110,4117,4065,4066,4116,4067,4118,4108,4115,4106,4113,4104,4111,4105,4112,4107,4114,4159,4160,4161,4162,4163,4164,4165,4166,4167,4168,4109,4109";
//var result = SortCommaSeparatedIntegers.Sort(numbers);
//Console.WriteLine(result);

//----------------------------------------------------------------------------
//FIND DUPLICATES
//FindDuplicateNumbersInTextFile.FindDuplicates();

//----------------------------------------------------------------------------
//FIND SUM OF DIGITS
//var sum = SumOfDigits.GetSumOfDigitsLinq2(123);
//var sum2 = SumOfDigits.GetSumOfDigitsLinq2(10);

//----------------------------------------------------------------------------
//JOSEPHUS PROBLEM
var result = JosephusProblem.GetLastStanding(100);
var result2 = JosephusProblem.GetLastStandingRecursive(100);
Console.WriteLine();
