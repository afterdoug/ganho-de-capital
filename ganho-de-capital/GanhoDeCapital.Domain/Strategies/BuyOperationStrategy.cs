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
        // Update weighted average price when buying using the formula:
        decimal totalCostBefore = weightedAverageCost * totalShares;
        decimal totalCostOperation = operation.UnitCost * operation.Quantity;
        
        totalShares += operation.Quantity;
        
        if (totalShares > 0)
            weightedAverageCost = (totalCostBefore + totalCostOperation) / totalShares;
        
        // No tax on buy operations
        return new TaxCalculationResult(0);
    }
}