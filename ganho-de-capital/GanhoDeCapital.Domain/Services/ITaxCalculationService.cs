using System.Collections.Generic;
using GanhoDeCapital.Domain.Entities;

namespace GanhoDeCapital.Domain.Services
{
    public interface ITaxCalculationService
    {
        IEnumerable<TaxCalculationResult> CalculateTaxes(IEnumerable<Operation> operations);
    }
}