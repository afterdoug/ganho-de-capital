using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Strategies;

public class BuyOperationStrategy : IOperationStrategy
{
    public TaxCalculationResult CalculateTax(
        StockOperation operation, 
        ref decimal weightedAverageCost, 
        ref int totalShares, 
        ref decimal accumulatedLoss)
    {
        CalculateWeightedAveragePrice(operation, ref weightedAverageCost, ref totalShares);

        // No tax on buy operations
        return new TaxCalculationResult(0);
    }

    private static void CalculateWeightedAveragePrice(StockOperation operation, ref decimal weightedAverageCost, ref int totalShares)
    {
        decimal totalCostBefore = weightedAverageCost * totalShares;
        decimal totalCostOperation = operation.UnitCost * operation.Quantity;

        totalShares += operation.Quantity;

        if (totalShares > 0)
            weightedAverageCost = (totalCostBefore + totalCostOperation) / totalShares;
    }
}