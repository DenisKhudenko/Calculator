using System.Globalization;
using System.Text.RegularExpressions;
using Calculator.BL.Exceptions;
using Calculator.BL.Services.Interfaces;

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
    
    public double Calculate()
    {
        // Сначала получаем лист токенов
        List<string> tokens = GetRegexMatches();

        // Инициализируем индекс
        int position = 0;
            
        // Далее парсим рекурсивно все уровни
        Node tree = ParseFirstLevel(tokens, ref position);
        
        // В конце рекурсивно считаем дерево
        return CalculateTree(tree);
    }
    
    // Получаем лист с последовательностью чисел и операторов в выражении
    private List<string> GetRegexMatches()
    {
        // Регулярное выражение для парсинга выражения
        Regex regex = new Regex(@"\d+(?:[.,]\d+)?|[\+\-\*\/\^\(\)]");
        
        // Если переданное выражение не подходит под паттерн, возвращаем исключение
        if(!regex.IsMatch(Expression)) throw new NotMatchingPatternException();
        
        // Получаем коллекцию вхождений из регулярного выражения, затем приводим к листу линком
        var matches = regex.Matches(Expression);
        return matches.Select(match => match.Value).ToList();
    }

    // 1 уровень дерева (низший приоритет), рассматриваем +-
    private Node ParseFirstLevel(List<string> tokens, ref int position)
    {
        // Идем рекурсивно вниз ко второму уровню
        Node left = ParseSecondLevel(tokens, ref position);

        // Далее раскидываем по сторонам плюс и минус
        while (position < tokens.Count 
               && (tokens[position] == "+" || tokens[position] == "-"))
        {
            string oper = tokens[position];
            position++;
                
            Node right = ParseSecondLevel(tokens, ref position);
            left = new Node(oper, left, right);
        }

        return left;
    }

    // 2 уровень дерева (средний приоритет), рассматриваем */^
    private Node ParseSecondLevel(List<string> tokens, ref int position)
    {
        // Разбирем числа и скобки
        Node left = ParseThirdLevel(tokens, ref position);

        // Далее разбираем по сторонам умножение, деление, степень
        while (position < tokens.Count 
               && (tokens[position] == "*" 
                   || tokens[position] == "/" 
                   || tokens[position] == "^"))
        {
            string oper = tokens[position];
            position++;
            
            Node right = ParseThirdLevel(tokens, ref position);
            left = new Node(oper, left, right);
        }

        return left;
    }

    // 2 уровень дерева (высший приоритет), рассматриваем скобки и числа
    private Node ParseThirdLevel(List<string> tokens, ref int position)
    {
        string token = tokens[position];
        position++;

        // Если скобка, то рекурсивно запускаем парсинг с первого уровня
        if (token == "(")
        {
            Node node = ParseFirstLevel(tokens, ref position);
            
            position++; // итерируем, чтобы пропустить закрывающую скобку
            return node;
        }

        // Возвращаем число
        return new Node(token);
    }

    // Рекурсивно считаем дерево
    private double CalculateTree(Node node)
    {
        // Если у узла не заполнены правый и левый узел, то это парсим значение как число
        if (node.Left == null 
            && node.Right == null) return double.Parse(node.Value.Replace(",", "."), CultureInfo.InvariantCulture);

        double left  = CalculateTree(node.Left); // рекурсивно считаем левый узел
        double right = CalculateTree(node.Right); // рекурсивно считаем правый узел

        return node.Value switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "^" => Math.Pow(left, right),
            _ => throw new InvalidOperatorException(node.Value)
        };
    }
}