using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace GanhoDeCapital.Domain.Services
{
    public class TaxCalculationService : ITaxCalculationService
    {

        public TaxCalculationService()
        {
        }

        public IEnumerable<TaxCalculationResult> CalculateTaxes(IEnumerable<StockOperation> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            var results = new List<TaxCalculationResult>();
            decimal weightedAverageCost = 0;
            int totalShares = 0;
            decimal accumulatedLoss = 0;

            foreach (var operation in operations)
            {
                if (operation.Operation == OperationType.Buy)
                {
                    // Update weighted average price when buying using the formula:
                    // new-weighted-average = ((current-shares * current-weighted-average) + (purchased-shares * purchase-price)) / (current-shares + purchased-shares)
                    decimal totalCostBefore = weightedAverageCost * totalShares;
                    decimal totalCostOperation = operation.UnitCost * operation.Quantity;
                    
                    totalShares += operation.Quantity;
                    
                    if (totalShares > 0)
                        weightedAverageCost = (totalCostBefore + totalCostOperation) / totalShares;
                    
                    // No tax on buy operations
                    results.Add(new TaxCalculationResult(0));
                }
                else if (operation.Operation == OperationType.Sell)
                {
                    // Calculate profit or loss
                    decimal operationTotal = operation.UnitCost * operation.Quantity;
                    decimal costBasis = weightedAverageCost * operation.Quantity;
                    decimal profit = operationTotal - costBasis;
                    
                    // Update total shares
                    totalShares -= operation.Quantity;
                    
                    // Calculate tax
                    decimal tax = 0;
                    
                    if (profit > 0)
                    {
                        // First, apply accumulated losses to reduce taxable profit
                        decimal taxableProfit = profit;
                        
                        // Apply exemption for operations below threshold (R$20,000.00)
                        if (operationTotal > BrazilianTaxRulesProvider.ExemptionThreshold)
                        {
                            if (accumulatedLoss > 0)
                            {
                                if (accumulatedLoss >= profit)
                                {
                                    // All profit is offset by previous losses
                                    accumulatedLoss -= profit;
                                    taxableProfit = 0;
                                }
                                else
                                {
                                    // Part of the profit is offset by previous losses
                                    taxableProfit = profit - accumulatedLoss;
                                    accumulatedLoss = 0;
                                }
                            }
                            // Apply 20% tax rate on the taxable profit
                            tax = Math.Round(taxableProfit * BrazilianTaxRulesProvider.TaxRate, 2);
                        }
                    }
                    else if (profit < 0)
                    {
                        // Accumulate losses for future operations
                        accumulatedLoss += Math.Abs(profit);
                    }
                    
                    results.Add(new TaxCalculationResult(tax));
                }
            }
            
            return results;
        }
    }
}