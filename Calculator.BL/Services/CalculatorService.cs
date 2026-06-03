using Calculator.BL.Interfaces;

namespace Calculator.BL;

public class CalculatorService() : ICalculatorService
{
    public string Expression { get; set; }
    
    public double Calculate()
    {
        return double.Parse(Expression);
    }
}