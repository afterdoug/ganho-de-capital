using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Providers;

namespace GanhoDeCapital.Domain.Specifications;

/// <summary>
/// Operations with total value below or equal to the threshold are exempt
/// </summary>
public class TaxExemptOperationSpecification : ISpecification<Operation>
{
    public bool IsSatisfiedBy(Operation operation)
    {
        // Buy operations are always exempt from taxes
        if (operation.OperationType == OperationType.Buy)
            return true;
        
        return operation.TotalCostOperation <= BrazilianTaxRulesProvider.ExemptionThreshold;
    }
}