namespace FeatureExamples;

public class DefaultBadExample
{
    public static void DefaultKeywordWrongUsage()
    {
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
    }
}

file class FirstClass
{
    public SecondClass SecondClass { get; set; }
}

file class SecondClass
{
    public int Id { get; set; }
}