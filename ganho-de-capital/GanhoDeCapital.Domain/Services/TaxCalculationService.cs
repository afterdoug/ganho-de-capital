using System;
using System.Collections.Generic;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Strategies;

namespace GanhoDeCapital.Domain.Services;

public class TaxCalculationService : ITaxCalculationService
{
    private readonly Dictionary<OperationType, IOperationStrategy> _strategies;

    public TaxCalculationService(IEnumerable<KeyValuePair<OperationType, IOperationStrategy>> strategies = null)
    {
        _strategies = new Dictionary<OperationType, IOperationStrategy>
            {
                { OperationType.Buy, new BuyOperationStrategy() },
                { OperationType.Sell, new SellOperationStrategy() }
            };
    }

    public IEnumerable<TaxCalculationResult> CalculateTaxes(IEnumerable<StockOperation> operations)
    {
        var results = new List<TaxCalculationResult>();
        var position = new Position();

        foreach (var operation in operations)
        {
            var strategy = _strategies.GetValueOrDefault(operation.Operation);

            var result = strategy!.CalculateTax(operation, ref position);

            results.Add(result);
        }
        
        return results;
    }
}
