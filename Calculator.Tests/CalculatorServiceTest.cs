using Calculator.BL.Services.Interfaces;
using Moq;
using Xunit;

namespace Calculator.Tests;

public class CalculatorServiceTests
{
    private readonly Mock<ICalculatorService> _mock = new();
    
    [Fact(DisplayName = "Проверка деления на 0")]
    public void DivisionByZero()
    {
        string expression = "1+2";
        
        // Arrange
        _mock.Setup(c => c.Calculate(expression))
            .Throws<DivideByZeroException>();
        
        // Assert
        Assert.Throws<DivideByZeroException>(delegate { _mock.Object.Calculate(expression); });
        // Как вариант использовать анонимный метод и сокращенно будет (() => _mock.Object.Calculate()
    }
    
    [Fact(DisplayName = "Метод Calculate вызывается единожды")]
    public void CalculateCalledOnce()
    {
        string expression = "1+2";
        
        // Act
        _mock.Object.Calculate(expression);

        // Assert
        _mock.Verify(c => c.Calculate(expression), Times.Once);
    }
    
    [Theory(DisplayName = "Проверка корректности вычисления")]
    [InlineData("2+3", 5.0)]
    [InlineData("2+3+(10*2)", 25.0)]
    [InlineData("2+3+(10*2)^2", 405.0)]
    [InlineData("2+3-(10/2,5)", 1.0)]
    [InlineData("2+3-(2.5/2,5)^2", 4.0)]
    public void CalculateExpressionReturnsExpected(string expression, double expected)
    {
        // Arrange
        _mock.Setup(c => c.Calculate(expression)).Returns(expected);

        // Act
        var result = _mock.Object.Calculate(expression);

        // Assert
        Assert.Equal(expected, result);
    }
}
