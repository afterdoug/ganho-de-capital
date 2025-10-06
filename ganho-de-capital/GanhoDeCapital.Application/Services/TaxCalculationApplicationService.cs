using System;
using System.Collections.Generic;
using System.Linq;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Application.Services;

public class TaxCalculationApplicationService : ITaxCalculationApplicationService
{
    private readonly ITaxCalculationService _taxCalculationService;

    public TaxCalculationApplicationService(ITaxCalculationService taxCalculationService)
    {
        _taxCalculationService = taxCalculationService ?? throw new ArgumentNullException(nameof(taxCalculationService));
    }

    public IEnumerable<TaxResultDto> CalculateTaxes(IEnumerable<StockOperationDto> operationDtos)
    {
        if (operationDtos == null)
            throw new ArgumentNullException(nameof(operationDtos));

        List<Operation> operations = MapToEntity(operationDtos);

        // Calculate taxes using domain service
        var taxResults = _taxCalculationService.CalculateTaxes(operations);

        // Map domain results back to DTOs
        return taxResults.Select(result => new TaxResultDto { Tax = result.Tax });
    }

    private static List<Operation> MapToEntity(IEnumerable<StockOperationDto> operationDtos)
    {
        return operationDtos.Select(dto => new Operation(
            ParseOperationType(dto.Operation),
            dto.UnitCost,
            dto.Quantity
        )).ToList();
    }

    private static OperationType ParseOperationType(string operation)
    {
        return operation.ToLower() switch
        {
            "buy" => OperationType.Buy,
            "sell" => OperationType.Sell,
            _ => throw new ArgumentException($"Invalid operation type: {operation}")
        };
    }
}