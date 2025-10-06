using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Strategies;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Strategies
{
    public class BuyOperationStrategyTests
    {
        private readonly BuyOperationStrategy _strategy;
        
        public BuyOperationStrategyTests()
        {
            _strategy = new BuyOperationStrategy();
        }
        
        [Fact]
        public void CalculateTax_ShouldUpdatePositionCorrectly()
        {
            // Arrange
            var position = new Position();
            int initialShares = position.TotalShares;
            
            // Use reflection to set initial weighted average cost
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, 10.0m);
            
            var operation = new StockOperation(OperationType.Buy, 15.0m, 100);
            
            // Act
            var result = _strategy.CalculateTax(operation, ref position);
            
            // Assert
            position.TotalShares.Should().Be(initialShares + operation.Quantity);
            position.WeightedAverageCost.Should().Be(15.0m); // Since starting from 0 shares
            result.Tax.Should().Be(0); // Buy operations have no tax
        }
        
        [Fact]
        public void CalculateTax_WithExistingPosition_ShouldCalculateWeightedAverageCorrectly()
        {
            // Arrange
            var position = new Position();
            
            // Setup initial position: 100 shares at 10.0 each
            position.AddShares(100);
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, 10.0m);
            
            // New buy operation: 50 shares at 20.0 each
            var operation = new StockOperation(OperationType.Buy, 20.0m, 50);
            
            // Expected weighted average: (100*10 + 50*20) / 150 = (1000 + 1000) / 150 = 2000 / 150 = 13.33...
            
            // Act
            var result = _strategy.CalculateTax(operation, ref position);
            
            // Assert
            position.TotalShares.Should().Be(150);
            position.WeightedAverageCost.Should().BeApproximately(13.33m, 0.01m);
            result.Tax.Should().Be(0);
        }
        
        [Theory]
        [InlineData(100, 10.0, 50, 20.0, 150, 13.33, 0)] // Initial 100@10, buy 50@20
        [InlineData(0, 0, 100, 15.0, 100, 15.0, 0)] // No initial shares, buy 100@15
        [InlineData(200, 25.0, 100, 15.0, 300, 21.67, 0)] // Initial 200@25, buy 100@15
        public void CalculateTax_WithVariousScenarios_ShouldCalculateCorrectly(
            int initialShares, decimal initialAvgCost, 
            int buyQuantity, decimal buyUnitCost,
            int expectedTotalShares, decimal expectedAvgCost, decimal expectedTax)
        {
            // Arrange
            var position = new Position();
            
            // Setup initial position
            position.AddShares(initialShares);
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, initialAvgCost);
            
            var operation = new StockOperation(OperationType.Buy, buyUnitCost, buyQuantity);
            
            // Act
            var result = _strategy.CalculateTax(operation, ref position);
            
            // Assert
            position.TotalShares.Should().Be(expectedTotalShares);
            position.WeightedAverageCost.Should().BeApproximately(expectedAvgCost, 0.01m);
            result.Tax.Should().Be(expectedTax);
        }
    }
}