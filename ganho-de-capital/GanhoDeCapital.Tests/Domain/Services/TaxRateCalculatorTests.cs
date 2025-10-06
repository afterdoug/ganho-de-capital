using FluentAssertions;
using GanhoDeCapital.Domain.Providers;
using GanhoDeCapital.Domain.Services;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Services
{
    public class TaxRateCalculatorTests
    {
        [Theory]
        [InlineData(1000, 200)] // 1000 * 0.20 = 200
        [InlineData(50000, 10000)] // 50000 * 0.20 = 10000
        [InlineData(123.45, 24.69)] // 123.45 * 0.20 = 24.69
        [InlineData(0, 0)] // 0 * 0.20 = 0
        public void CalculateTax_ShouldApplyCorrectTaxRate(decimal profit, decimal expectedTax)
        {
            // Act
            decimal calculatedTax = TaxRateCalculator.CalculateTax(profit);
            
            // Assert
            calculatedTax.Should().Be(expectedTax);
        }
        
        [Fact]
        public void CalculateTax_ShouldUseRateFromProvider()
        {
            // Arrange
            decimal profit = 1000m;
            decimal expectedTax = profit * BrazilianTaxRulesProvider.TaxRate;
            
            // Act
            decimal calculatedTax = TaxRateCalculator.CalculateTax(profit);
            
            // Assert
            calculatedTax.Should().Be(expectedTax);
        }
        
        [Fact]
        public void CalculateTax_ShouldRoundToTwoDecimalPlaces()
        {
            // Arrange
            decimal profit = 1000.123m;
            decimal expectedTax = decimal.Round(profit * BrazilianTaxRulesProvider.TaxRate, 2);
            
            // Act
            decimal calculatedTax = TaxRateCalculator.CalculateTax(profit);
            
            // Assert
            calculatedTax.Should().Be(expectedTax);
        }
    }
}