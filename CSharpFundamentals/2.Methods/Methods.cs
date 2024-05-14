namespace CSharpFundamentals;

public class Methods
{
    /// <summary>
    /// Prints the text to the console
    /// </summary>
    /// <param name="text">The text to print</param>
    public void Print(string text)
    {
        Console.WriteLine(text);
    }

    /// <summary>
    /// Static way to print the text to the console
    /// </summary>
    /// <param name="text">The text to print</param>
    public static void PrintStatic(string text)
    {
        Console.WriteLine(text);
    }

    /// <summary>
    /// Default values for parameters
    /// </summary>
    /// <param name="number"></param>
    /// <param name="text"></param>
    public static void DefaultValues(int number, string text = "Hello")
    {
        Console.WriteLine($"Number: {number}, Text: {text}");
    }

    /// <summary>
    /// Return based on condition
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ReturnWithIf(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "Text is empty";
        }

        return text;
    }

    /// <summary>
    /// Addition of two numbers
    /// </summary>
    /// <param name="number1"></param>
    /// <param name="number2"></param>
    /// <returns></returns>
    public static int Add(int number1, int number2)
    {
        return number1 + number2;
    }

    /// <summary>
    /// Second overload of Add method
    /// </summary>
    /// <param name="number1"></param>
    /// <param name="number2"></param>
    /// <returns></returns>
    public static int Add(short number1, short number2)
    {
        return number1 + number2;
    }

    /// <summary>
    /// Addition of two numbers with out parameter
    /// </summary>
    /// <param name="number1"></param>
    /// <param name="number2"></param>
    /// <param name="result"></param>
    public static void Add(int number1, int number2, out int result)
    {
        result = number1 + number2;
    }

    /// <summary>
    /// Addition of two numbers with ref parameter
    /// </summary>
    /// <param name="number1"></param>
    /// <param name="number2"></param>
    /// <param name="result"></param>
    public static void AddRef(int number1, int number2, ref int result)
    {
        result = number1 + number2;
    }

    /// <summary>
    /// Addition of multiple numbers with params
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static int Add(params int[] numbers)
    {
        int result = 0;

        foreach (var number in numbers)
        {
            result += number;
        }

        return result;
    }
    
    /// <summary>
    /// Addition of multiple numbers with LINQ
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static int AddLinq(params int[] numbers) => numbers.Sum();
}