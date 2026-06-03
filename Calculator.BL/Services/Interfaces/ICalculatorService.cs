namespace Calculator.BL.Interfaces;

public interface ICalculatorService
{
    string Expression { get; set; }
    
    double Calculate();
}