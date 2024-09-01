using TestProject;
using Xunit.Abstractions;

namespace XUnitTests;

public class CalculatorTests
{
    private Calculator _calculator = new Calculator();
    private readonly ITestOutputHelper _output;

    public CalculatorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData(10, 2, 8)]
    [InlineData(3, 2, 1)]
    [InlineData(1, 2, -1)]
    public void Subtract_TwoNumbers_Success(int firstNumber, int secondNumber, int expected)
    {
        //Act
        var result = _calculator.Subtraction(firstNumber, secondNumber);
        _output.WriteLine("subtraction run.");

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Subtract_TwoNumbers_Failure()
    {
        //Arrange
        var firstNumber = 15;
        var secondNumber = 15;
        var expected = 4;

        //Act
        var result = _calculator.Subtraction(firstNumber, secondNumber);

        //Assert
        Assert.NotEqual(expected, result);
    }

    [Fact]
    public void Division_WithZero_ShouldThrowException()
    {
        var firstNumber = 1;
        var secondNumber = 0;

        Assert.Throws<DivideByZeroException>(() => _calculator.Division(firstNumber, secondNumber));
    }

    [Theory, CombinatorialData]
    public void AddingRandom_TwoNumbers_Fail(
        [CombinatorialValues(1, 2, 3)] int firstNumber,
        [CombinatorialValues(4, 5, 6)] int secondNumber,
        [CombinatorialValues(20, 30, 40)] int expected)
    {
        //Act
        var result = _calculator.Addition(firstNumber, secondNumber);
        //Assert
        Assert.NotEqual(expected, result);
    }

    [Theory, CombinatorialData]
    public void AddingRandom_TwoNumbers_Fail_Range(
        [CombinatorialRange(from :1, count: 2)] int firstNumber,
        [CombinatorialRange(4, 2)] int secondNumber,
        [CombinatorialRange(10, 3)] int expected)
    {
        //Act
        var result = _calculator.Addition(firstNumber, secondNumber);
        //Assert
        Assert.NotEqual(expected, result);
    }

    [Theory, CombinatorialData]
    public void Divide_TwoRandomNumbers_NoExceptions(int firstNumber, int secondNumber)
    {
        //Act
        Assert.Throws<DivideByZeroException>(() => _calculator.Division(firstNumber, secondNumber));
    }
}