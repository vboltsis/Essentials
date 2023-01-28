namespace XUnitTests;

internal class Calculator
{
    internal int Addition(params int[] numbers)
    {
        return numbers.Sum();
    }

    internal int Subtraction(int firstNumber, int secondNumber)
    {
        return firstNumber - secondNumber;
    }

    internal int Division(int firstNumber, int secondNumber)
    {
        return firstNumber / secondNumber;
    }
}
