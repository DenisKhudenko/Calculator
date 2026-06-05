namespace Calculator.BL.Exceptions;

public class CalculatorException : Exception
{
    public string Code { get; init; }

    public CalculatorException(string message, string code)
    {
        Code = code;
    }
}

public class NotMatchingPatternException()
    : CalculatorException("Выражение не подходит под шаблон калькулятора", "NotMatchingPattern");

public class NotMatchingBracketsException()
    : CalculatorException("Скобки в выражении не закрыты", "NotMatchingBrackets");