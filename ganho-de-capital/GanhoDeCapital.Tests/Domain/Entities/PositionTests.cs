using System;
using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Entities
{
    public class PositionTests
    {
        [Fact]
        public void AddShares_ShouldIncreaseTotalShares()
        {
            // Arrange
            var position = new Position();
            int initialShares = position.TotalShares;
            int sharesToAdd = 100;
            
            // Act
            position.AddShares(sharesToAdd);
            
            // Assert
            position.TotalShares.Should().Be(initialShares + sharesToAdd);
        }
        
        [Fact]
        public void RemoveShares_ShouldDecreaseTotalShares()
        {
            // Arrange
            var position = new Position();
            position.AddShares(100); // Setup initial shares
            int initialShares = position.TotalShares;
            int sharesToRemove = 50;
            
            // Act
            position.RemoveShares(sharesToRemove);
            
            // Assert
            position.TotalShares.Should().Be(initialShares - sharesToRemove);
        }
        
        [Fact]
        public void UpdateTotalCostBefore_ShouldCalculateCorrectly()
        {
            // Arrange
            var position = new Position();
            position.AddShares(100);
            
            // Use reflection to set the weighted average cost since it's a private setter
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, 10.5m);
            
            // Act
            position.UpdateTotalCostBefore();
            
            // Assert
            position.TotalCostBefore.Should().Be(10.5m * 100);
        }
        
        [Fact]
        public void CalculateWeightedAveragePrice_ShouldCalculateCorrectly()
        {
            // Arrange
            var position = new Position();
            position.AddShares(100);
            
            // Use reflection to set the weighted average cost
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, 10.0m);
            
            position.UpdateTotalCostBefore(); // TotalCostBefore = 10.0 * 100 = 1000
            
            decimal totalCostOperation = 500.0m; // 50 shares at 10.0 each
            position.AddShares(50); // Now 150 shares total
            
            // Act
            position.CalculateWeightedAveragePrice(totalCostOperation);
            
            // Assert
            position.WeightedAverageCost.Should().Be(10.0m); // (1000 + 500) / 150 = 10.0
        }
        
        [Fact]
        public void CalculateWeightedAveragePrice_WithZeroShares_ShouldNotChangePrice()
        {
            // Arrange
            var position = new Position();
            
            // Use reflection to set the weighted average cost
            typeof(Position).GetProperty("WeightedAverageCost")
                .SetValue(position, 10.0m);
            
            // Act
            position.CalculateWeightedAveragePrice(500.0m);
            
            // Assert
            position.WeightedAverageCost.Should().Be(10.0m); // Should remain unchanged
        }
        
        [Fact]
        public void IncreaseAccumulatedLoss_ShouldAddAbsoluteValueToAccumulatedLoss()
        {
            // Arrange
            var position = new Position();
            decimal initialLoss = 100.0m;
            
            // Use reflection to set the accumulated loss
            typeof(Position).GetProperty("AccumulatedLoss")
                .SetValue(position, initialLoss);
            
            decimal negativeProfit = -50.0m;
            
            // Act
            position.IncreaseAccumulatedLoss(negativeProfit);
            
            // Assert
            position.AccumulatedLoss.Should().Be(initialLoss + 50.0m);
        }
        
        [Fact]
        public void ReduceAccumulatedLoss_ShouldSubtractFromAccumulatedLoss()
        {
            // Arrange
            var position = new Position();
            decimal initialLoss = 100.0m;
            
            // Use reflection to set the accumulated loss
            typeof(Position).GetProperty("AccumulatedLoss")
                .SetValue(position, initialLoss);
            
            decimal profit = 30.0m;
            
            // Act
            position.ReduceAccumulatedLoss(profit);
            
            // Assert
            position.AccumulatedLoss.Should().Be(initialLoss - profit);
        }
        
        [Fact]
        public void ResetAccumulatedLoss_ShouldSetAccumulatedLossToZero()
        {
            // Arrange
            var position = new Position();
            
            // Use reflection to set the accumulated loss
            typeof(Position).GetProperty("AccumulatedLoss")
                .SetValue(position, 100.0m);
            
            // Act
            // ResetAccumulatedLoss is internal, so we need to use reflection to call it
            typeof(Position).GetMethod("ResetAccumulatedLoss", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(position, null);
            
            // Assert
            position.AccumulatedLoss.Should().Be(0);
        }
    }
}