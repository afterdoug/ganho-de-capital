using FluentAssertions;
using GanhoDeCapital.Domain.Providers;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Providers
{
    public class BrazilianTaxRulesProviderTests
    {
        [Fact]
        public void TaxRate_ShouldBeTwentyPercent()
        {
            // Act & Assert
            BrazilianTaxRulesProvider.TaxRate.Should().Be(0.20m);
        }
        
        [Fact]
        public void ExemptionThreshold_ShouldBeTwentyThousand()
        {
            // Act & Assert
            BrazilianTaxRulesProvider.ExemptionThreshold.Should().Be(20000.00m);
        }
    }
}