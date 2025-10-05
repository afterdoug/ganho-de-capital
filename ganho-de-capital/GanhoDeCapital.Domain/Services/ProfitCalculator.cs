using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Services;

public class ProfitCalculator
{
    protected ProfitCalculator()
    {
    }

    public static decimal CalculateProfit(StockOperation operation, decimal weightedAverageCost)
    {
        if (operation.Operation != OperationType.Sell)
            return 0;
            
        decimal costBasis = weightedAverageCost * operation.Quantity;
        decimal operationTotal = operation.UnitCost * operation.Quantity;
        
        return operationTotal - costBasis;
    }
    
    public static decimal CalculateTaxableProfit(decimal profit, ref decimal accumulatedLoss)
    {
        if (profit <= 0)
            return 0;
            
        decimal taxableProfit = profit;
        
        if (accumulatedLoss > 0)
        {
            if (accumulatedLoss >= profit)
            {
                accumulatedLoss -= profit;
                taxableProfit = 0;
            }
            else
            {
                taxableProfit = profit - accumulatedLoss;
                accumulatedLoss = 0;
            }
        }
        
        return taxableProfit;
    }
}