using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
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

//ArrayList arrayList = new ArrayList();
//arrayList.Add(1);
//arrayList.Add("hello world");

//foreach (var item in arrayList)
//{
//    Console.WriteLine(item);
//}


int[] array = new int[2];
var humans = new List<IHuman>();
List<int> list = new List<int>(humans.Count);//set correct initial capacity

for (int i = 0; i < humans.Count; i++)
{
    list.Add(i);
}

Collection<int> collection = new Collection<int>();
Dictionary<int, string> dictionary = new Dictionary<int, string>(10);
HashSet<int> hashSet = new HashSet<int>(10);
Stack<int> stack = new Stack<int>(10);
Queue<int> queue = new Queue<int>(10);
ConcurrentDictionary<int, string> concurrentDictionary = new ConcurrentDictionary<int, string>();
ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
ConcurrentStack<int> concurrentStack = new ConcurrentStack<int>();

//var student = new Student();
//var teacher = new Teacher();

//list.Add(student);
//list.Add(teacher);

Console.WriteLine();

class Student : Human
{

}

class Teacher : Human
{

}

class Human : IHuman
{

}

interface IHuman
{

}