namespace GanhoDeCapital.Domain.Services
{
    public interface ITaxRulesProvider
    {
        decimal TaxRate { get; }
        decimal ExemptionThreshold { get; }
    }
}