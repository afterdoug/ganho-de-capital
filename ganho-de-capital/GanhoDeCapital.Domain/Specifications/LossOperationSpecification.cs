namespace GanhoDeCapital.Domain.Specifications;

/// <summary>
/// Operation results in loss if the selling price is lower than the cost basis
/// </summary>
public class LossOperationSpecification : ISpecification<(decimal operationTotal, decimal costBasis)>
{
    public bool IsSatisfiedBy((decimal operationTotal, decimal costBasis) entity)
    {
        var (operationTotal, costBasis) = entity;
        
        return operationTotal < costBasis;
    }
}