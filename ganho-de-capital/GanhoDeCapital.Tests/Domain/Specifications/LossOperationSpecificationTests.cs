using FluentAssertions;
using GanhoDeCapital.Domain.Specifications;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Specifications
{
    public class LossOperationSpecificationTests
    {
        private readonly LossOperationSpecification _specification;
        
        public LossOperationSpecificationTests()
        {
            _specification = new LossOperationSpecification();
        }
        
        [Theory]
        [InlineData(900, 1000, true)] // Operation total < cost basis (loss)
        [InlineData(1000, 1000, false)] // Operation total = cost basis (no loss)
        [InlineData(1100, 1000, false)] // Operation total > cost basis (profit)
        public void IsSatisfiedBy_ShouldIdentifyLossOperations(
            decimal operationTotal, decimal costBasis, bool expectedResult)
        {
            // Arrange
            var entity = (operationTotal, costBasis);
            
            // Act
            bool result = _specification.IsSatisfiedBy(entity);
            
            // Assert
            result.Should().Be(expectedResult);
        }
        
        [Fact]
        public void IsSatisfiedBy_WithLoss_ShouldReturnTrue()
        {
            // Arrange
            decimal operationTotal = 800m;
            decimal costBasis = 1000m;
            var entity = (operationTotal, costBasis);
            
            // Act
            bool result = _specification.IsSatisfiedBy(entity);
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_WithProfit_ShouldReturnFalse()
        {
            // Arrange
            decimal operationTotal = 1200m;
            decimal costBasis = 1000m;
            var entity = (operationTotal, costBasis);
            
            // Act
            bool result = _specification.IsSatisfiedBy(entity);
            
            // Assert
            result.Should().BeFalse();
        }
    }
}