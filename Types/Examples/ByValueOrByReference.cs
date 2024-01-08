namespace Types;

//EVERYTHING IS PASSED BY VALUE IN C# UNLESS YOU USE THE REF KEYWORD

//if we pass a value type, we are passing a copy of the value
//if we pass a reference type, we are passing a copy of the reference

internal class ByValueOrByReference
{
    public static void ByValue(int number, int? nullable, DateTime date, string text,
        List<int> list, Person person, byte[] bytes, PersonStruct personStruct)
    {
        // These will NOT change the value of the variables passed in
        number = 2; 
        nullable = 2; 
        date = new DateTime(2222, 2, 2); 
        text = "2"; 
        personStruct.Age = 2; 
        personStruct.Name = "2";

        // This will NOT change the value of the variable passed in since we are assigning a new object
        list = new List<int>(); 
        list.Add(2);

        //These will change the value of the variable passed in
        person.Age = 2;
        person.Name = "2";
        bytes[0] = 2;
    }

    public static void ByReference(ref int number, ref int? nullable, ref DateTime date, ref string text,
        ref List<int> list, ref Person person, ref byte[] bytes, ref PersonStruct personStruct)
    {
        //All of these will change the value of the variable passed in
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
