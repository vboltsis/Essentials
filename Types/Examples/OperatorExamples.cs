namespace FeatureExamples;

internal class OperatorExamples
{
    public static void Example()
    {
        //ternary operator
        int age = 20;
        string result = age >= 18 ? "Adult" : "Minor";
        Console.WriteLine(result); // Outputs: Adult

        //null-coalescing operator
        string input = null;
        string result2 = input ?? "Default Value";
        Console.WriteLine(result2); // Outputs: Default Value

        //Null-Coalescing Assignment Operator 
        List<int> numbers = null;
        numbers ??= new List<int>();
        numbers.Add(1);
        Console.WriteLine(numbers.Count); // Outputs: 1


        //Null-conditional Operator
        User user = null;
        string name = user?.Name;
        Console.WriteLine(name); // Outputs: (null)

        int[] numbers2 = null;
        int? firstNumber = numbers2?[0];
        Console.WriteLine(firstNumber); // Outputs: (null)
    }
}
