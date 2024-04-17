using System.Collections.Frozen;

namespace Types;

//.NET FRAMEWORK 1 --> 2 --> 4.8.2 -- windows only
//.NET CORE 1 --> 2.2 --> 3.1 --> 5.0 --> 6.0 --> 7.0 -- cross platform
public class Variables
{
    public static void Method()
    {
        int number = 1;
        short number2 = 1;
        long number3 = 1;
        byte number4 = 1;
        sbyte number5 = 1;
        ushort number6 = 1;
        uint number7 = 1;
        ulong number8 = 1;

        string text = "takis";
        char character = 't';

        bool boolean = true;
        bool boolean2 = false;
        bool boolean3 = true && false;//AND
        bool boolean4 = true || false;//OR
        bool boolean5 = !true;//NOT

        decimal decimalNumber = 1.1m;
        double doubleNumber = 1.1;
        float floatNumber = 1.1f;

        DateTime date = new DateTime(1111, 1, 1);
        DateTimeOffset dateTimeOffset = new DateTimeOffset(date);
        DateOnly dateOnly = new DateOnly(2020, 1, 1);
        TimeOnly timeOnly = new TimeOnly(1, 1, 1);

        int[] array = new int[10];
        List<int> list = new List<int> { 1, 2, 3 };
        Dictionary<int, string> dictionary = new Dictionary<int, string> { { 1, "1" }, { 2, "2" } };
        HashSet<int> hashSet = new HashSet<int> { 1, 2, 3, 1, 1, 1 };
        (int, string) tuple = (1, "1");
        FrozenDictionary<int, string> frozenDictionary = dictionary.ToFrozenDictionary();
        FrozenSet<int> frozenSet = hashSet.ToFrozenSet();
        

        //covariance
        List<IAnimal> animal = new List<IAnimal>
        {
            new Dog(),
            new Cat()
        };
    }
}
