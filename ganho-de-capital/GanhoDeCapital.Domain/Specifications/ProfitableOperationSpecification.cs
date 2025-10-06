namespace GanhoDeCapital.Domain.Specifications;

/// <summary>
/// Operation is profitable if the selling price is higher than the cost basis
/// </summary>
public class ProfitableOperationSpecification : ISpecification<(decimal operationTotal, decimal costBasis)>
{
    public bool IsSatisfiedBy((decimal operationTotal, decimal costBasis) entity)
    {
        var (operationTotal, costBasis) = entity;
        
        return operationTotal > costBasis;
    }
}