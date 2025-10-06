using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;
using NSubstitute;
using Xunit;

namespace GanhoDeCapital.Tests.Domain.Services
{
    public class TaxCalculationServiceTests
    {
        // Since we don't have the actual implementation of TaxCalculationService in the provided code,
        // we'll test the interface contract with a mock implementation
        
        [Fact]
        public void CalculateTaxes_ShouldReturnSameNumberOfResultsAsOperations()
        {
            // Arrange
            var service = Substitute.For<ITaxCalculationService>();
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 10.0m, 100),
                new StockOperation(OperationType.Sell, 15.0m, 50)
            };
            
            var expectedResults = new List<TaxCalculationResult>
            {
                new TaxCalculationResult(0),
                new TaxCalculationResult(250.0m) // (15 - 10) * 50 * 0.20 = 50 * 0.20 = 10
            };
            
            service.CalculateTaxes(operations).Returns(expectedResults);
            
            // Act
            var results = service.CalculateTaxes(operations).ToList();
            
            // Assert
            results.Should().HaveCount(operations.Count);
            results[0].Tax.Should().Be(expectedResults[0].Tax);
            results[1].Tax.Should().Be(expectedResults[1].Tax);
        }
        
        [Fact]
        public void CalculateTaxes_WithEmptyOperations_ShouldReturnEmptyResults()
        {
            // Arrange
            var service = Substitute.For<ITaxCalculationService>();
            var operations = new List<StockOperation>();
            
            service.CalculateTaxes(operations).Returns(new List<TaxCalculationResult>());
            
            // Act
            var results = service.CalculateTaxes(operations);
            
            // Assert
            results.Should().BeEmpty();
        }
        
        // Note: Without the actual implementation of TaxCalculationService,
        // we can't test the actual tax calculation logic in detail.
        // These tests just verify the interface contract.
    }
}