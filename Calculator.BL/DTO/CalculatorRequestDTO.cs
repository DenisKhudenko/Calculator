namespace Calculator.BL.DTO;

/// <summary>
/// Предоставляет маппер запроса данных для расчета калькулятора
/// </summary>
public class CalculatorRequestDTO
{
    /// <summary>
    /// Выражение для расчета
    /// </summary>
    public string Expression { get; set; }
}