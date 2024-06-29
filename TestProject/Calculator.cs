namespace TestProject;

public class Calculator
{
    public int Addition(params int[] numbers)
    {
        return numbers.Sum();
    }

    public int Subtraction(int firstNumber, int secondNumber)
    {
        return firstNumber - secondNumber;
    }

    public int Division(int firstNumber, int secondNumber)
    {
        return firstNumber / secondNumber;
    }
}
