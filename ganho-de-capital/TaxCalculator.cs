using System;
using System.Collections.Generic;

namespace GanhoDeCapital
{
    public class TaxCalculator
    {
        private const decimal TaxRate = 0.20m; // 20% tax rate on profits
        private const decimal ExemptionLimit = 20000.00m; // Operations below R$20,000.00 are exempt

        public List<TaxResult> CalculateTax(List<StockOperation> operations)
        {
            var results = new List<TaxResult>();
            decimal weightedAverageCost = 0;
            int totalShares = 0;
            decimal accumulatedLoss = 0;

            foreach (var operation in operations)
            {
                if (operation.Operation.ToLower() == "buy")
                {
                    // Update weighted average price when buying
                    decimal totalCostBefore = weightedAverageCost * totalShares;
                    decimal totalCostOperation = operation.UnitCost * operation.Quantity;
                    totalShares += operation.Quantity;
                    weightedAverageCost = (totalCostBefore + totalCostOperation) / totalShares;
                    
                    // No tax on buy operations
                    results.Add(new TaxResult { Tax = 0 });
                }
                else if (operation.Operation.ToLower() == "sell")
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
                        // Apply accumulated losses to reduce taxable profit
                        profit = Math.Max(0, profit - accumulatedLoss);
                        accumulatedLoss = Math.Max(0, accumulatedLoss - (operationTotal - costBasis));
                        
                        // Apply exemption for operations below R$20,000
                        if (operationTotal > ExemptionLimit)
                        {
                            tax = profit * TaxRate;
                        }
                    }
                    else
                    {
                        // Accumulate losses for future operations
                        accumulatedLoss += Math.Abs(profit);
                    }
                    
                    results.Add(new TaxResult { Tax = Math.Round(tax, 2) });
                }
            }
            
            return results;
        }
    }
}