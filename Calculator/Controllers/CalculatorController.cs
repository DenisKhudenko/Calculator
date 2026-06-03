using Calculator.BL.DTO;
using Calculator.BL.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers;

/// <summary>
/// Получение данных для калькулятора
/// </summary>
[ApiController]
[Route("calculator")]
public class CalculatorController(ILogger<CalculatorController> logger) : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger = logger;
    
    /// <summary>
    /// Ввод строки для расчета в калькуляторе
    /// </summary>
    [HttpPost("calculator")]
    public OkObjectResult Calculate([FromBody] CalculatorRequestDTO dto)
    {
        return Ok(dto
            .MapCalculatorFromRequestDto()
            .Calculate()
            .MapResultCalculatorToResponseDto());
    }
}