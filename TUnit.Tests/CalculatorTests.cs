using TestProject;

namespace TUnit.Tests;

public class CalculatorTests
{
    private readonly Calculator _calculator = new Calculator();

    [Before(Class)]
    public static async Task BeforeClass()
    {
        await Task.CompletedTask;
    }

    [After(Class)]
    public static async Task AfterClass()
    {
        await Task.CompletedTask;
    }

    [Test, Retry(2), DisplayName("Addition")]
    public async Task Add_WhenCalled_ReturnsSum()
    {
        var result = _calculator.Add(2, 3);
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task Subtract_WhenCalled_ReturnsDifference()
    {
        var result = _calculator.Subtract(5, 3);
        await Assert.That(result).IsEqualTo(2);
    }

    [Test]
    [Arguments(10, 2, 5)]
    [Arguments(20, 4, 5)]
    public async Task Divide_WhenCalled_ReturnsQuotient(int firstNumber, int secondNumber, int expected)
    {
        var result = _calculator.Divide(firstNumber, secondNumber);
        await Assert.That(result).IsEqualTo(expected);
    }

}
