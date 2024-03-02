namespace Types;

public class RecordsVsClasses
{
    public static void Class()
    {
        var stu1 = new Student { Name = "Takis" };
        var stu2 = new Student { Name = "Takis" };

        var set = new HashSet<Student> { };
        set.Add(stu1);
        set.Add(stu2);

        var contains = set.Contains(new Student { Name = "Takis" });
        
        stu1.Print();
        var result = stu1 == stu2;
        var result2 = stu1.Equals(stu2);

        Console.WriteLine(stu1.ToString());
    }

    public static void Record()
    {
        var stu1 = new RecordStudent { Name = "Takis" };
        var stu2 = new RecordStudent { Name = "Takis" };

        var set = new HashSet<RecordStudent> { };
        set.Add(stu1);
        set.Add(stu2);
        
        var contains = set.Contains(new RecordStudent { Name = "Takis" });

        var result = stu1 == stu2;
        var result2 = stu1.Equals(stu2);

        Console.WriteLine(stu1.ToString());
    }

    public static void Struct()
    {
        var stu1 = new StructStudent { Name = "Takis" };
        var stu2 = new StructStudent { Name = "Takis" };

        var set = new HashSet<StructStudent> { };
        set.Add(stu1);
        set.Add(stu2);

        //var result = stu1 == stu2; //won't compile unless you write the operator yourself
        var result2 = stu1.Equals(stu2);

        Console.WriteLine(stu1.ToString());
    }

    public static void RecordStruct()
    {
        var stu1 = new RecordStructStudent { Name = "Takis" };
        var stu2 = new RecordStructStudent { Name = "Takis" };

        var set = new HashSet<RecordStructStudent> { };
        set.Add(stu1);
        set.Add(stu2);

        var result = stu1 == stu2;
        var result2 = stu1.Equals(stu2);

        Console.WriteLine(stu1.ToString());
    }
}

record RecordStudent
{
    public string Name { get; set; }
}

record struct RecordStructStudent
{
    public string Name { get; set; }
}

class Student : Human
{
    public string Name { get; set; }
}

struct StructStudent
{
    public string Name { get; set; }
}

class Human
{
    public void Print()
    {
        Console.WriteLine("Human");
    }
}