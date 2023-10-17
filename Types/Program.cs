using FeatureExamples;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Types;

//AsyncExample.BoomAsync();
SpanExamples.ParseDateFromStringSpan("monday:2020/1/1");
SpanExamples.ParseDateFromString("monday:2020/1/1");
ExpressionTreesExample.LinqExample();

var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };


var newList = list.Where(i => i > 5).ToList();
var newList2 = list.FindAll(i => i > 5);


var sw = Stopwatch.GetTimestamp();

await Task.WhenAll(AsyncExample.PrintAsyncOne(), AsyncExample.PrintAsyncTwo());
//await AsyncExample.PrintAsyncOne();
//await AsyncExample.PrintAsyncTwo();

//another async thing

var elapsed = Stopwatch.GetElapsedTime(sw);

Console.WriteLine(elapsed);

Console.ReadKey();











//TypeExamples.Types();

//var queue = new ChannelTaskQueue();

//queue.StartTaskConsumers(3);

//// Enqueue some tasks
//for (int i = 0; i < 10_000; i++)
//{
//    var local = i;
//    await queue.EnqueueTask(async () => {
//        await Task.Delay(1000);
//        Console.WriteLine($"Task {local} completed!");
//    });
//}

//await Task.Delay(30000);

//Console.WriteLine();

//int number = 1;
//int? nullable = null;
//DateTime date = new DateTime(1111, 1, 1);
//string text = "1";
//List<int> list = new List<int> { 1, 1 };
//Person person = new Person { Name = "1", Age = 1 };
//byte[] bytes = new byte[4];
//PersonStruct personStruct = new PersonStruct { Name = "1", Age = 1 };

//ByValueOrByReference.ByValue(number, nullable, date, text, list, person, bytes, personStruct);
//ByValueOrByReference.ByReference(ref number, ref nullable, ref date, ref text, ref list, ref person, ref bytes, ref personStruct);

//BOXING
//object number = 1;
//object number2 = 1;

//var result = number == number2; //false
//var result2 = number.Equals(number2); //true

//DateTime date = new DateTime(1111, 1, 1);
//DateTimeOffset dateTimeOffset = new DateTimeOffset(date);
//DateOnly dateOnly = new DateOnly(2020,1,1);
//TimeOnly timeOnly = new TimeOnly(10, 10);

//Enumerables!!

//Plumber.WillThrowWhenRun();
//var res1 = Test.StopwatchStart();
//var res2 = Test.GetTimestamp();

//var array = new Takis[] { 
//    new Takis { Id = 3, Age = 3 },
//    new Takis { Id = 1, Age = 1 },
//    new Takis { Id = 2, Age = 2 },
//};

//Array.Sort(array, (a, b) => a.Id.CompareTo(b.Id));

//var numbers = new List<int> { 8, 2, 3, 4, 5, 6, 7, 1 };
//numbers.Sort();

//Console.ReadKey();

struct Takis
{
    public int Id { get; set; }
    public int Age { get; set; }
}

