using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;
using GanhoDeCapital.Domain.Specifications;

namespace GanhoDeCapital.Domain.Strategies;

public class SellOperationStrategy : IOperationStrategy
{
    private readonly TaxExemptOperationSpecification _taxExemptSpec;
    private readonly ProfitableOperationSpecification _profitableSpec;
    private readonly LossOperationSpecification _lossSpec;

    public SellOperationStrategy()
    {
        _taxExemptSpec = new TaxExemptOperationSpecification();
        _profitableSpec = new ProfitableOperationSpecification();
        _lossSpec = new LossOperationSpecification();
    }

    public TaxCalculationResult CalculateTax(
        StockOperation operation,
        ref Position position)
    {
        decimal tax = 0;

        // Calculate profit or loss
        var (profit, operationTotal, costBasis) = ProfitCalculator.CalculateProfit(operation, position.WeightedAverageCost);

        //Update Total Shares
        position.RemoveShares(operation.Quantity);

        // Check if operation is profitable and Check if operation is exempt from taxes
        if (_profitableSpec.IsSatisfiedBy((operationTotal, costBasis))
            && !_taxExemptSpec.IsSatisfiedBy((operation, position.WeightedAverageCost)))
        {
            decimal taxableProfit = ProfitCalculator.CalculateTaxableProfit(profit, ref position);
            
            tax = TaxRateCalculator.CalculateTax(taxableProfit);
        }
        // Check if operation results in loss
        else if (_lossSpec.IsSatisfiedBy((operationTotal, costBasis)))
        {
            position.IncreaseAccumulatedLoss(profit);
        }

        return new TaxCalculationResult(tax);
    }
}
