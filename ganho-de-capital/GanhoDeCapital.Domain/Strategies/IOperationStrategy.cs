using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Strategies
{
    public interface IOperationStrategy
    {
        TaxCalculationResult CalculateTax(
            Operation operation, 
            ref Position position);
    }
}