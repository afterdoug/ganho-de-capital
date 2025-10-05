using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Domain.Specifications
{
    public class ProfitableOperationSpecification : ISpecification<(StockOperation operation, decimal weightedAverageCost)>
    {
        public bool IsSatisfiedBy((StockOperation operation, decimal weightedAverageCost) entity)
        {
            var (operation, weightedAverageCost) = entity;

            // Calculate profit or loss
            decimal profit = ProfitCalculator.CalculateProfit(operation, weightedAverageCost);

            // Operation is profitable if the selling price is higher than the cost basis
            return profit > 0;
        }
    }
}