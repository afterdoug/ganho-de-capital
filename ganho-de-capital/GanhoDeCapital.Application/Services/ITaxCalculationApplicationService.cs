using System.Collections.Generic;
using GanhoDeCapital.Application.DTOs;

namespace GanhoDeCapital.Application.Services
{
    public interface ITaxCalculationApplicationService
    {
        IEnumerable<TaxResultDto> CalculateTaxes(IEnumerable<StockOperationDto> operations);
    }
}