namespace GanhoDeCapital.Domain.Specifications;

public class ProfitableOperationSpecification : ISpecification<(decimal operationTotal, decimal costBasis)>
{
    public bool IsSatisfiedBy((decimal operationTotal, decimal costBasis) entity)
    {
        var (operationTotal, costBasis) = entity;
        
        // Operation is profitable if the selling price is higher than the cost basis
        return operationTotal > costBasis;
    }
}