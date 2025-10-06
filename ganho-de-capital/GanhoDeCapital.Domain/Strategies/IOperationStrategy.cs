using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Strategies
{
    public interface IOperationStrategy
    {
        TaxCalculationResult CalculateTax(
            StockOperation operation, 
            ref Position position);
    }
}