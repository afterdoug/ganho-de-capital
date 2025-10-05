using System;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Providers;
using GanhoDeCapital.Domain.Services;
using GanhoDeCapital.Domain.Specifications;

namespace GanhoDeCapital.Domain.Strategies
{
    public class SellOperationStrategy : IOperationStrategy
    {
        private readonly TaxExemptOperationSpecification _taxExemptSpec;
        private readonly ProfitableOperationSpecification _profitableSpec;
        private readonly LossOperationSpecification _lossSpec;

        public SellOperationStrategy()
        {
            _taxExemptSpec = new TaxExemptOperationSpecification();
            _profitableSpec = new ProfitableOperationSpecification();
            _lossSpec = new LossOperationSpecification();
        }

        public TaxCalculationResult CalculateTax(
            StockOperation operation, 
            ref decimal weightedAverageCost, 
            ref int totalShares, 
            ref decimal accumulatedLoss)
        {
            // Calculate profit or loss
            decimal profit = ProfitCalculator.CalculateProfit(operation, weightedAverageCost);

            // Update total shares
            totalShares -= operation.Quantity;

            // Calculate tax
            decimal tax = 0;

            // Check if operation is profitable and Check if operation is exempt from taxes
            if (_profitableSpec.IsSatisfiedBy((operation, weightedAverageCost))
                && !_taxExemptSpec.IsSatisfiedBy((operation, weightedAverageCost)))
            {
                // Calculate taxable profit after applying accumulated losses
                decimal taxableProfit = ProfitCalculator.CalculateTaxableProfit(profit, ref accumulatedLoss);

                // Apply tax rate on the taxable profit
                tax = Math.Round(taxableProfit * BrazilianTaxRulesProvider.TaxRate, 2);
            }
            // Check if operation results in loss
            else if (_lossSpec.IsSatisfiedBy((operation, weightedAverageCost)))
            {
                // Accumulate losses for future operations
                accumulatedLoss += Math.Abs(profit);
            }
            
            return new TaxCalculationResult(tax);
        }
    }
}