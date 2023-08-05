using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Types;

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
Test.Method();

var array = new Takis[] { 
    new Takis { Id = 3, Age = 3 },
    new Takis { Id = 1, Age = 1 },
    new Takis { Id = 2, Age = 2 },
};

Array.Sort(array, (a, b) => a.Id.CompareTo(b.Id));

var numbers = new List<int> { 8, 2, 3, 4, 5, 6, 7, 1 };
numbers.Sort();

Console.ReadKey();

struct Takis
{
    public int Id { get; set; }
    public int Age { get; set; }
}

class Test
{
    public static void Method()
    {
        var size = 100_000_000;
        var array = GC.AllocateUninitializedArray<int>(size);

        Span<int> array1 = stackalloc int[10];
        for (int i = 0; i < size; i++)
        {
            array[i] = i;
        }

        foreach(double item in array)
        {
            Console.WriteLine(item);
        }
    }

    public static void Method2(in Takis test)
    {
        Console.WriteLine();
    }
}