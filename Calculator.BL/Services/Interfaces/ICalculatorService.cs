namespace Calculator.BL.Interfaces;

public interface ICalculatorService
{
    string Expression { get; set; }
    int Position { get; set; }
    
    double Calculate();
}