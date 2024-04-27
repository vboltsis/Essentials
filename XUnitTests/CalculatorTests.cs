namespace XUnitTests;

public class CalculatorTests
{
    private Calculator _calculator = new Calculator();

    [Theory]
    [InlineData(10, 2, 8)]
    [InlineData(3, 2, 1)]
    [InlineData(1, 2, -1)]
    public void Subtract_TwoNumbers_Success(int firstNumber, int secondNumber, int expected)
    {
        //Act
        var result = _calculator.Subtraction(firstNumber, secondNumber);

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
}