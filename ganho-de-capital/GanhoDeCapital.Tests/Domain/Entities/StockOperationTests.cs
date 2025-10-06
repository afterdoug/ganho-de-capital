using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Entities
{
    public class StockOperationTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            OperationType operationType = OperationType.Buy;
            decimal unitCost = 10.5m;
            int quantity = 100;
            
            // Act
            var operation = new Operation(operationType, unitCost, quantity);
            
            // Assert
            operation.OperationType.Should().Be(operationType);
            operation.UnitCost.Should().Be(unitCost);
            operation.Quantity.Should().Be(quantity);
        }
        
        [Fact]
        public void TotalCostOperation_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal unitCost = 10.5m;
            int quantity = 100;
            var operation = new Operation(OperationType.Buy, unitCost, quantity);
            
            // Act
            decimal totalCost = operation.TotalCostOperation;
            
            // Assert
            totalCost.Should().Be(unitCost * quantity);
        }
        
        [Theory]
        [InlineData(OperationType.Buy, 15.75, 200, 3150)]
        [InlineData(OperationType.Sell, 25.50, 50, 1275)]
        [InlineData(OperationType.Buy, 100, 1, 100)]
        public void TotalCostOperation_WithVariousInputs_ShouldCalculateCorrectly(
            OperationType operationType, decimal unitCost, int quantity, decimal expectedTotal)
        {
            // Arrange
            var operation = new Operation(operationType, unitCost, quantity);
            
            // Act
            decimal totalCost = operation.TotalCostOperation;
            
            // Assert
            totalCost.Should().Be(expectedTotal);
        }
    }
}