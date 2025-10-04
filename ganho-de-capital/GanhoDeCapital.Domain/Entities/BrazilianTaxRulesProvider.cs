using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Infrastructure.Services
{
    public static class BrazilianTaxRulesProvider
    {
        // 20% tax rate on profits as specified in the requirements
        public static decimal TaxRate => 0.20m;
        
        // Operations below R$20,000.00 are exempt from taxes
        public static decimal ExemptionThreshold => 20000.00m;
    }
}