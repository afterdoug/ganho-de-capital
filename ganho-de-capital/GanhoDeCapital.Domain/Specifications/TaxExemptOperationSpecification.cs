using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Providers;

namespace GanhoDeCapital.Domain.Specifications;

public class TaxExemptOperationSpecification : ISpecification<(StockOperation operation, decimal weightedAverageCost)>
{
    public bool IsSatisfiedBy((StockOperation operation, decimal weightedAverageCost) entity)
    {
        var (operation, _) = entity;
        
        // Buy operations are always exempt from taxes
        if (operation.Operation == OperationType.Buy)
            return true;
            
        // Operations with total value below or equal to the threshold are exempt
        decimal operationTotal = operation.UnitCost * operation.Quantity;
        return operationTotal <= BrazilianTaxRulesProvider.ExemptionThreshold;
    }
}