namespace Calculator.BL.Services.Interfaces;

public interface ICalculatorService
{
    string Expression { get; set; }
    
    double Calculate();
}