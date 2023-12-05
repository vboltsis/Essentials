using System.Collections.Concurrent;

namespace Types;

internal class TypeExamples
{
    public static void Types()
    {
        /*
bool: a Boolean value that can be either true or false.
byte: an 8-bit unsigned integer.
char: a 16-bit Unicode character.
decimal: a 128-bit precise decimal value.
double: a 64-bit double-precision floating-point number.
float: a 32-bit single-precision floating-point number.
int: a 32-bit signed integer. 
long: a 64-bit signed integer.

The main difference between signed and unsigned integers is the range of values that they can represent.

A signed integer is a whole number that can be positive, negative, or zero,
and it uses the leftmost bit to represent the sign (0 for positive and 1 for negative).
For example, an int is a signed integer that can range from -2147483648 to 2147483647.

On the other hand, an unsigned integer is a whole number that can only be positive or zero,
and it does not use the leftmost bit to represent the sign.
Instead, all of the bits are used to represent the magnitude of the number.
For example, a uint (unsigned integer) can range from 0 to 4294967295.

Signed integers are useful when you need to represent both positive and negative values,
while unsigned integers are useful when you only need to represent positive values or zero.

C# also has unsigned versions of the other integer types: byte, short, and long.
These types are similar to their signed counterparts,
but they use all of their bits to represent the magnitude of the number and
do not use the leftmost bit to represent the sign.
 */
        //sortedDictionary - sortedList
        //sortedSet
        //stack - queue

        //TYPE -- NAME OF VARIABLE -- VALUE
        bool flag = true;
        bool result = false;

        byte age = 35;
        byte maxSpeed = 255;

        int count = 10;
        int number = -5;

        char letter = 'A';
        char symbol = '#';
        int character = 'W';

        double pi = 3.14159;
        double average = 86.5;

        decimal price = 19.99m;
        decimal balance = 100.00m;

        float temperature = 72.5f;
        float distance = 3.9f;

        long population = 8000000000;
        long scandals = 999999999;

        short temperature2 = -5;
        short temperature3 = 6;
        uint temperature4 = 6;
        ushort temperature5 = 6;
        ulong temperature6 = 6;

        DateTime today = DateTime.Now;
        DateTime todayUtc = DateTime.UtcNow;
        DateTime birthday = new DateTime(1985, 3, 28);
        DateTimeOffset dateTimeOffset = DateTimeOffset.Parse("2024-01-01 10:10:10 +04:00");
        DateTimeOffset dateTimeOffset2 = DateTimeOffset.Parse("2024-01-01 10:10:10 +02:00");

        DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        TimeOnly timeOnly = TimeOnly.FromDateTime(DateTime.Now);

        int[] numbers = new int[10];
        List<int> list = new List<int>(9);
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);
        list.Add(6);
        list.Add(7);
        list.Add(8);
        list.Add(9);

        var dictionary = new Dictionary<int, string>(10) { { 1, "one" }, { 2, "two" } };
        HashSet<int> hashSet = new HashSet<int>(10) { 1, 2, 3, 4, 5 };
        Queue<int> queue = new Queue<int>();
        Stack<int> stack = new Stack<int>();

        ConcurrentDictionary<int, string> keyValuePairs = new ConcurrentDictionary<int, string>(Environment.ProcessorCount, 10);
        ConcurrentQueue<int> ints = new ConcurrentQueue<int>();
        ConcurrentBag<int> ints2 = new ConcurrentBag<int>();
        ConcurrentStack<int> ints3 = new ConcurrentStack<int>();

        //.NET FRAMEWORK 1 - 2 - 4.8
        //.NET CORE 1 - 2.2 - 3.1 - 5 - 6 - 7 - 8

        //CLASS --> REFERENCE TYPE --> HEAP
        //STRUCT --> VALUE TYPE --> STACK
    }
}
