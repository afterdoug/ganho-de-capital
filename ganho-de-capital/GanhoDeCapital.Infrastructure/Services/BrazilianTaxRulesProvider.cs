using GanhoDeCapital.Domain.Services;

namespace GanhoDeCapital.Infrastructure.Services
{
    public class BrazilianTaxRulesProvider : ITaxRulesProvider
    {
        // 20% tax rate on profits as specified in the requirements
        public decimal TaxRate => 0.20m;
        
        // Operations below R$20,000.00 are exempt from taxes
        public decimal ExemptionThreshold => 20000.00m;
    }
}