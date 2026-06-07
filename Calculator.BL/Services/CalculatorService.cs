using System.Text.RegularExpressions;
using Calculator.BL.Exceptions;
using Calculator.BL.Interfaces;

namespace Calculator.BL.Services;

// Узел дерева, из него собираем дерево рекурсией
class Node(string value, Node? left = null, Node? right = null)
{
    public readonly string Value = value;
    public readonly Node? Left = left;
    public readonly Node? Right = right;
}

public class CalculatorService() : ICalculatorService
{
    public string Expression { get; set; }
    public int Position { get; set; }
    
    private List<string> _tokens;
    
    public double Calculate()
    {
        // Сначала получаем лист токенов
        _tokens = GetRegexMatches();

        // Далее парсим рекурсивно все уровни
        Node tree = ParseFirstLevel();
        
        // В конце рекурсивно считаем дерево
        return CalculateTree(tree);
    }
    
    // Получаем лист с последовательностью чисел и операторов в выражении
    private List<string> GetRegexMatches()
    {
        // Регулярное выражение для парсинга выражения
        Regex regex = new Regex(@"\d+(?:\.\d+)?|[\+\-\*\/\^\(\)]");
        
        // Если переданное выражение не подходит под паттерн, возвращаем исключение
        if(!regex.IsMatch(Expression)) throw new NotMatchingPatternException();
        
        // Получаем коллекцию вхождений из регулярного выражения, затем приводим к листу линком
        var matches = regex.Matches(Expression);
        return matches.Select(match => match.Value).ToList();
    }

    // 1 уровень дерева (низший приоритет), рассматриваем +-
    private Node ParseFirstLevel()
    {
        // Идем рекурсивно вниз ко второму уровню
        Node left = ParseSecondLevel();

        // Далее раскидываем по сторонам плюс и минус
        while (Position < _tokens.Count 
               && (_tokens[Position] == "+" || _tokens[Position] == "-"))
        {
            string oper = _tokens[Position++];
            Node right = ParseSecondLevel();
            left = new Node(oper, left, right);
        }

        return left;
    }

    // 2 уровень дерева (средний приоритет), рассматриваем */^
    private Node ParseSecondLevel()
    {
        // Разбирем числа и скобки
        Node left = ParseThirdLevel();

        // Далее разбираем по сторонам умножение, деление, степень
        while (Position < _tokens.Count 
               && (_tokens[Position] == "*" 
                   || _tokens[Position] == "/" 
                   || _tokens[Position] == "^"))
        {
            string oper = _tokens[Position++];
            Node right = ParseThirdLevel();
            left = new Node(oper, left, right);
        }

        return left;
    }

    // 2 уровень дерева (высший приоритет), рассматриваем скобки и числа
    private Node ParseThirdLevel()
    {
        string token = _tokens[Position++];

        // Если скобка, то рекурсивно запускаем парсинг с первого уровня
        if (token == "(")
        {
            Node node = ParseFirstLevel();
            
            Position++; // итерируем, чтобы пропустить закрывающую скобку
            return node;
        }

        // Возвращаем число
        return new Node(token);
    }

    // Рекурсивно считаем дерево
    private double CalculateTree(Node node)
    {
        // Если у узла не заполнены правый и левый узел, то это парсим значение как число
        if (node.Left == null && node.Right == null) return double.Parse(node.Value);

        double left  = CalculateTree(node.Left); // рекурсивно считаем левый узел
        double right = CalculateTree(node.Right); // рекурсивно считаем правый узел

        return node.Value switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "^" => Math.Pow(left, right),
            _ => throw new InvalidOperatorException(char.Parse(node.Value))
        };
    }
}