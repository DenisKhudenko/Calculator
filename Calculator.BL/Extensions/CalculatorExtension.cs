using Calculator.BL.DTO;
using Calculator.BL.Services;
using Calculator.BL.Services.Interfaces;

namespace Calculator.BL.Extensions;

public static class CalculatorExtension
{
    public static ICalculatorService MapCalculatorFromRequestDto(this CalculatorRequestDTO dto)
    {
        return new CalculatorService() { Expression = dto.Expression};
    }  
    
    public static CalculatorResponseDTO MapResultCalculatorToResponseDto(this double result)
    {
        return new CalculatorResponseDTO() { Result = result };
    } 
}