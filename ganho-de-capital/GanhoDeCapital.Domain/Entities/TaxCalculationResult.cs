namespace GanhoDeCapital.Domain.Entities
{
    public class TaxCalculationResult
    {
        public decimal Tax { get; private set; }

        public TaxCalculationResult(decimal tax)
        {
            Tax = tax >= 0 ? decimal.Round(tax, 2) : 0;
        }
    }
}