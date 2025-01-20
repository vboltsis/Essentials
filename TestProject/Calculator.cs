namespace TestProject;

public class Calculator
{
    public int Add(params int[] numbers)
    {
        return numbers.Sum();
    }

    public int Subtract(int firstNumber, int secondNumber)
    {
        return firstNumber - secondNumber;
    }

    public int Divide(int firstNumber, int secondNumber)
    {
        return firstNumber / secondNumber;
    }
}
