using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Providers;
using GanhoDeCapital.Domain.Specifications;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Specifications
{
    public class TaxExemptOperationSpecificationTests
    {
        private readonly TaxExemptOperationSpecification _specification;
        
        public TaxExemptOperationSpecificationTests()
        {
            _specification = new TaxExemptOperationSpecification();
        }
        
        [Fact]
        public void IsSatisfiedBy_WithBuyOperation_ShouldReturnTrue()
        {
            // Arrange
            var operation = new Operation(OperationType.Buy, 100m, 1000);
            
            // Act
            bool result = _specification.IsSatisfiedBy(operation);
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_WithSellOperationBelowThreshold_ShouldReturnTrue()
        {
            // Arrange
            decimal unitCost = 10m;
            int quantity = 1000; // Total: 10,000 which is below the 20,000 threshold
            var operation = new Operation(OperationType.Sell, unitCost, quantity);
            
            // Act
            bool result = _specification.IsSatisfiedBy(operation);
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_WithSellOperationEqualToThreshold_ShouldReturnTrue()
        {
            // Arrange
            decimal unitCost = 20m;
            int quantity = 1000; // Total: 20,000 which is equal to the threshold
            var operation = new Operation(OperationType.Sell, unitCost, quantity);
            
            // Act
            bool result = _specification.IsSatisfiedBy(operation);
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_WithSellOperationAboveThreshold_ShouldReturnFalse()
        {
            // Arrange
            decimal unitCost = 25m;
            int quantity = 1000; // Total: 25,000 which is above the 20,000 threshold
            var operation = new Operation(OperationType.Sell, unitCost, quantity);
            
            // Act
            bool result = _specification.IsSatisfiedBy(operation);
            
            // Assert
            result.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(OperationType.Buy, 100, 500, true)] // Buy operations are always exempt
        [InlineData(OperationType.Sell, 10, 1000, true)] // 10,000 < threshold
        [InlineData(OperationType.Sell, 20, 1000, true)] // 20,000 = threshold
        [InlineData(OperationType.Sell, 30, 1000, false)] // 30,000 > threshold
        public void IsSatisfiedBy_WithVariousOperations_ShouldDetermineExemptionCorrectly(
            OperationType operationType, decimal unitCost, int quantity, bool expectedResult)
        {
            // Arrange
            var operation = new Operation(operationType, unitCost, quantity);
            
            // Act
            bool result = _specification.IsSatisfiedBy(operation);
            
            // Assert
            result.Should().Be(expectedResult);
        }
    }
}