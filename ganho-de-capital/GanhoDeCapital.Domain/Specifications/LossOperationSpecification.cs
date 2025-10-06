using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Domain.Specifications;

public class LossOperationSpecification : ISpecification<(decimal operationTotal, decimal costBasis)>
{
    public bool IsSatisfiedBy((decimal operationTotal, decimal costBasis) entity)
    {
        var (operationTotal, costBasis) = entity;
        // Operation results in loss if the selling price is lower than the cost basis
        return operationTotal < costBasis;
    }
}