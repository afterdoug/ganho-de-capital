using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Services;

public class ProfitCalculator
{
    protected ProfitCalculator()
    {
    }

    public static (decimal, decimal, decimal) CalculateProfit(Operation operation, decimal weightedAverageCost)
    {
        if (operation.OperationType != OperationType.Sell)
            return default;
            
        decimal costBasis = weightedAverageCost * operation.Quantity;
        decimal operationTotal = operation.UnitCost * operation.Quantity;
        decimal profit = operationTotal - costBasis;

        return (profit, operationTotal, costBasis);
    }
    
    public static decimal CalculateTaxableProfit(decimal profit, ref Position position)
    {
        if (profit <= 0)
            return 0;
            
        decimal taxableProfit = profit;
        
        if (position.AccumulatedLoss > 0)
        {
            if (position.AccumulatedLoss >= profit)
            {
                position.ReduceAccumulatedLoss(profit);
                taxableProfit = 0;
            }
            else
            {
                taxableProfit = profit - position.AccumulatedLoss;
                position.ResetAccumulatedLoss();
            }
        }
        
       return taxableProfit;
    }
}