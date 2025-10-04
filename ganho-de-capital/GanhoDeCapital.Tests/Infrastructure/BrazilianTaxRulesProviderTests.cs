using FluentAssertions;
using GanhoDeCapital.Infrastructure.Services;
using Xunit;

namespace GanhoDeCapital.Tests.Infrastructure
{
    public class BrazilianTaxRulesProviderTests
    {
        private readonly BrazilianTaxRulesProvider _taxRulesProvider;

        public BrazilianTaxRulesProviderTests()
        {
            _taxRulesProvider = new BrazilianTaxRulesProvider();
        }

        [Fact]
        public void TaxRate_ShouldBe20Percent()
        {
            // Act & Assert
            _taxRulesProvider.TaxRate.Should().Be(0.20m);
        }

        [Fact]
        public void ExemptionThreshold_ShouldBe20000()
        {
            // Act & Assert
            _taxRulesProvider.ExemptionThreshold.Should().Be(20000.00m);
        }
    }
}