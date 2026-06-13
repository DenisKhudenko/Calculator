using Calculator.BL.Services.Interfaces;
using Moq;
using Xunit;

namespace Calculator.Tests;

public class CalculatorServiceTests
{
    private readonly Mock<ICalculatorService> _mock = new();
    
    [Fact(DisplayName = "Корректная установка выражения")]
    public void CorrectSetValueExpression()
    {
        // Arrange
        _mock.SetupProperty(c => c.Expression);

        // Act
        _mock.Object.Expression = "1+1";

        // Assert
        Assert.Equal("1+1", _mock.Object.Expression);
    }
    
    [Fact(DisplayName = "Проверка деления на 0")]
    public void DivisionByZero()
    {
        // Arrange
        _mock.Setup(c => c.Expression).Returns("1/0");
        _mock.Setup(c => c.Calculate())
            .Throws<DivideByZeroException>();
        
        // Assert
        Assert.Throws<DivideByZeroException>(() => _mock.Object.Calculate());
    }
    
    [Fact(DisplayName = "Метод Calculate вызывается единожды")]
    public void CalculateCalledOnce()
    {
        // Act
        _mock.Object.Calculate();

        // Assert
        _mock.Verify(c => c.Calculate(), Times.Once);
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
        _mock.Setup(c => c.Expression).Returns(expression);
        _mock.Setup(c => c.Calculate()).Returns(expected);

        // Act
        var result = _mock.Object.Calculate();

        // Assert
        Assert.Equal(expected, result);
    }
}
