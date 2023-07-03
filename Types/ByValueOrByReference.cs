namespace Types;

internal class ByValueOrByReference
{
    public static void ByValue(int number, int? nullable, DateTime date, string text,
        List<int> list, Person person, byte[] bytes, PersonStruct personStruct)
    {
        number = 2;
        nullable = 2;
        date = new DateTime(2222, 2, 2);
        text = "2";
        list = new List<int>();
        list.Add(2);
        person.Age = 2;
        person.Name = "2";
        bytes[0] = 2;
        personStruct.Age = 2;
        personStruct.Name = "2";
    }

    public static void ByReference(ref int number, ref int? nullable, ref DateTime date, ref string text,
        ref List<int> list, ref Person person, ref byte[] bytes, ref PersonStruct personStruct)
    {
        number = 2;
        nullable = 2;
        date = new DateTime(2222, 2, 2);
        text = "2";
        list = new List<int>();
        list.Add(2);
        person.Age = 2;
        person.Name = "2";
        bytes[0] = 2;
        personStruct.Age = 2;
        personStruct.Name = "2";
    }
}

internal class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

internal struct PersonStruct
{
    public string Name { get; set; }
    public int Age { get; set; }
}
