using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Specifications
{
    public class ProfitableOperationSpecification : ISpecification<(StockOperation operation, decimal weightedAverageCost)>
    {
        public bool IsSatisfiedBy((StockOperation operation, decimal weightedAverageCost) entity)
        {
            var (operation, weightedAverageCost) = entity;
            
            // Only sell operations can be profitable
            if (operation.Operation != OperationType.Sell)
                return false;
                
            decimal costBasis = weightedAverageCost * operation.Quantity;
            decimal operationTotal = operation.UnitCost * operation.Quantity;
            
            // Operation is profitable if the selling price is higher than the cost basis
            return operationTotal > costBasis;
        }
    }
}