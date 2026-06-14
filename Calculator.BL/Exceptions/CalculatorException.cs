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

public class InvalidOperatorException(string oper)
    : CalculatorException($"Неизвестный оператор: {oper}", "NotMatchingBrackets")
{
    public string Operator { get; init; } = oper;
}