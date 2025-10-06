using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Strategies;

public class BuyOperationStrategy : IOperationStrategy
{
    public TaxCalculationResult CalculateTax(
        StockOperation operation, 
        ref Position position)
    {
        position.UpdateTotalCostBefore();
        position.AddShares(operation.Quantity);
        position.CalculateWeightedAveragePrice(operation.TotalCostOperation);

        // No tax on buy operations
        return new TaxCalculationResult(0);
    }
}