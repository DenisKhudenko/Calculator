namespace Calculator.BL.Exceptions;

public class CalculatorException : Exception
{
    public string Code { get; init; }

    protected CalculatorException(string message, string code)
    {
        Code = code;
    }
}

public class NotMatchingPatternException()
    : CalculatorException("Выражение не подходит под шаблон калькулятора", "NotMatchingPattern");

public class InvalidOperatorException(char oper)
    : CalculatorException($"Неизвестный оператор: {oper}", "NotMatchingBrackets")
{
    public char Operator { get; init; } = oper;
}