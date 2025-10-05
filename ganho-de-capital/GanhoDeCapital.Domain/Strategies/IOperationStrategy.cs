using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Strategies
{
    public interface IOperationStrategy
    {
        TaxCalculationResult CalculateTax(
            StockOperation operation, 
            ref decimal weightedAverageCost, 
            ref int totalShares, 
            ref decimal accumulatedLoss);
    }
}