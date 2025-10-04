using System;
using System.Collections.Generic;
using System.Linq;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Application.Services
{
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

            // Map DTOs to domain entities
            var operations = operationDtos.Select(dto => new StockOperation(
                ParseOperationType(dto.Operation),
                dto.UnitCost,
                dto.Quantity
            )).ToList();

            // Calculate taxes using domain service
            var taxResults = _taxCalculationService.CalculateTaxes(operations);

            // Map domain results back to DTOs
            return taxResults.Select(result => new TaxResultDto { Tax = result.Tax });
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
}