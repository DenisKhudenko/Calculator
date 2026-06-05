using System.Text.RegularExpressions;
using Calculator.BL.Exceptions;
using Calculator.BL.Interfaces;

namespace Calculator.BL.Services;

public class CalculatorService() : ICalculatorService
{
    public string Expression { get; set; }
    
    public double Calculate()
    {
        var matches = GetRegexMatches();

        var operators = new List<char>();
        var output = new List<double>();
        
        for (var index = 0; index < matches.Count; index++)
        {
            var match = matches[index];
            
            if (double.TryParse(match, out var value))
            {
                output.Add(value);
            }
            else if (match == "(")
            {
                output.Add(CalculateExpressionOnBracketRecursive(matches, ++index));
            }
            else if(match == "-" || match == "+")
            {
                operators.Add(char.Parse(match));
            }
            else if (match == "*" || match == "/" || match == "^")
            {
                var newValue = CalculateTwoNearNumber(output[output.Count - 1], value, char.Parse(match));
                output.Remove(output.Count - 1);
                output.Remove(output.Count - 2);
                output.Add(newValue);
            }
        }
        
        return CalculateExpression(output, operators);
    }

    private List<string> GetRegexMatches()
    {
        var regex = new Regex(@"\d+(?:\.\d+)?|[\+\-\*\/\^\(\)]");
        
        if(!regex.IsMatch(Expression)) throw new NotMatchingPatternException();
        
        var matches = regex.Matches(Expression);

        List<string> list = new List<string>();
        
        foreach (Match match in matches) { list.Add(match.Value); } 
        return list;
    }

    private double CalculateExpressionOnBracketRecursive(List<string> matches, int index)
    {
        var operators = new List<char>();
        var output = new List<double>();
        
        while (index < matches.Count)
        {
            var match = matches[index];
            
            if (double.TryParse(match, out var value))
            {
                output.Add(value);
            } 
            else if (match == "-" || match == "+")
            {
                operators.Add(char.Parse(match));
            }
            else if (match == "*" || match == "/" || match == "^")
            {
                var newValue = CalculateTwoNearNumber(output[output.Count - 1], value, char.Parse(match));
                output.Remove(output.Count - 1);
                output.Remove(output.Count - 2);
                output.Add(newValue);
            }
            else if (match == "(")
            {
                output.Add(CalculateExpressionOnBracketRecursive(matches, ++index));
            } 
            else if (match == ")")
            {
                return CalculateExpressionOnBracketRecursive(matches, index + 1);
            }
            
            index++;
        }
        
        throw new NotMatchingBracketsException();
    }

    private double CalculateExpression(List<double> output, List<char> operators)
    {
        var indexOperator = 0;
        var result = output[0];
        for (var index = 1; index < output.Count; index++)
        {
            var value = output[index];
            switch (operators[indexOperator])
            {
                case '*': result *= value; break;
                case '/': result /= value; break;
                case '+': result += value; break;
                case '-': result -= value; break;
                case '^':
                    for (int i = 1; i < (value - 1); i++) { result *= result; } break;   
            }

            indexOperator++;
        }
        
        return result;
    }

    private double CalculateTwoNearNumber(double value1, double value2, char op)
    {
        switch (op)
        {
            case '*': value1 *= value2; break;
            case '/': value1 /= value2; break;
            case '+': value1 += value2; break;
            case '-': value1 -= value2; break;
            case '^':
                for (int i = 1; i < (value2 - 1); i++) { value1 *= value1; } break;   
        } 
        return value1;
    }
}