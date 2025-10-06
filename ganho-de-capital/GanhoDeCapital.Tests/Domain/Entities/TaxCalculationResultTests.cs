using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Entities
{
    public class TaxCalculationResultTests
    {
        [Theory]
        [InlineData(100.456, 100.46)] // Tests rounding up
        [InlineData(100.454, 100.45)] // Tests rounding down
        [InlineData(0, 0)] // Tests zero
        [InlineData(-10, 0)] // Tests negative value becoming zero
        public void Constructor_ShouldRoundAndHandleNegativeValues(decimal inputTax, decimal expectedTax)
        {
            // Act
            var result = new TaxCalculationResult(inputTax);
            
            // Assert
            result.Tax.Should().Be(expectedTax);
        }
        
        [Fact]
        public void Constructor_WithPositiveTax_ShouldRoundToTwoDecimalPlaces()
        {
            // Arrange
            decimal tax = 123.456m;
            
            // Act
            var result = new TaxCalculationResult(tax);
            
            // Assert
            result.Tax.Should().Be(123.46m);
        }
        
        [Fact]
        public void Constructor_WithNegativeTax_ShouldReturnZero()
        {
            // Arrange
            decimal tax = -50.0m;
            
            // Act
            var result = new TaxCalculationResult(tax);
            
            // Assert
            result.Tax.Should().Be(0);
        }
    }
}