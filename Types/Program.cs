using FeatureExamples;

//UpperCaseStringCollection.Example();
Console.WriteLine();

var myClass = new FirstClass
{
    SecondClass = new SecondClass
    {
        Id = 0
    }
};


if (myClass.SecondClass?.Id != default)
{
    Console.WriteLine("it is not 0");
}
else
{
    Console.WriteLine("it is 0");
}

class FirstClass
{
    public SecondClass SecondClass { get; set; }
}

class SecondClass
{
    public int Id { get; set; }
}