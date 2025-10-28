using System;
using GanhoDeCapital.Domain.Providers;

namespace GanhoDeCapital.Domain.Services;

public class TaxRateCalculator
{
    protected TaxRateCalculator()
    {
    }

    public static decimal ApplyTax(decimal taxableProfit)
    {
        return Math.Round(taxableProfit * BrazilianTaxRulesProvider.TaxRate, 2);
    }
}