using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Specifications;

public class LossOperationSpecification : ISpecification<(StockOperation operation, decimal weightedAverageCost)>
{
    public bool IsSatisfiedBy((StockOperation operation, decimal weightedAverageCost) entity)
    {
        var (operation, weightedAverageCost) = entity;
        
        // Only sell operations can result in loss
        if (operation.Operation != OperationType.Sell)
            return false;
            
        decimal costBasis = weightedAverageCost * operation.Quantity;
        decimal operationTotal = operation.UnitCost * operation.Quantity;
        
        // Operation results in loss if the selling price is lower than the cost basis
        return operationTotal < costBasis;
    }
}