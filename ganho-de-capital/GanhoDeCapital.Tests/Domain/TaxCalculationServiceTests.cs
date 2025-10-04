using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;
using NSubstitute;
using Xunit;

namespace GanhoDeCapital.Tests.Domain
{
    public class TaxCalculationServiceTests
    {
        private readonly ITaxRulesProvider _taxRulesProvider;
        private readonly TaxCalculationService _taxCalculationService;

        public TaxCalculationServiceTests()
        {
            // Setup mock for tax rules provider
            _taxRulesProvider = Substitute.For<ITaxRulesProvider>();
            _taxRulesProvider.TaxRate.Returns(0.20m); // 20% tax rate
            _taxRulesProvider.ExemptionThreshold.Returns(20000.00m); // R$20,000.00 threshold

            _taxCalculationService = new TaxCalculationService(_taxRulesProvider);
        }

        [Fact]
        public void CalculateTaxes_Case1_ShouldReturnCorrectTaxes()
        {
            // Arrange - Case #1
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 10.00m, 100),
                new StockOperation(OperationType.Sell, 15.00m, 50),
                new StockOperation(OperationType.Sell, 15.00m, 50)
            };

            // Act
            var results = _taxCalculationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0); // Buy operation - no tax
            results[1].Tax.Should().Be(0); // Sell below threshold - no tax
            results[2].Tax.Should().Be(0); // Sell below threshold - no tax
        }

        [Fact]
        public void CalculateTaxes_Case2_ShouldReturnCorrectTaxes()
        {
            // Arrange - Case #2
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 10.00m, 10000),
                new StockOperation(OperationType.Sell, 20.00m, 5000),
                new StockOperation(OperationType.Sell, 5.00m, 5000)
            };

            // Act
            var results = _taxCalculationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0); // Buy operation - no tax
            results[1].Tax.Should().Be(10000.00m); // Profit of R$50,000, 20% tax = R$10,000
            results[2].Tax.Should().Be(0); // Loss of R$25,000 - no tax
        }

        [Fact]
        public void CalculateTaxes_Case3_ShouldReturnCorrectTaxes()
        {
            // Arrange - Case #3
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 10.00m, 10000),
                new StockOperation(OperationType.Sell, 5.00m, 5000),
                new StockOperation(OperationType.Sell, 20.00m, 3000)
            };

            // Act
            var results = _taxCalculationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0); // Buy operation - no tax
            results[1].Tax.Should().Be(0); // Loss of R$25,000 - no tax
            results[2].Tax.Should().Be(1000.00m); // Profit of R$30,000, offset by R$25,000 loss, tax on R$5,000 = R$1,000
        }

        [Fact]
        public void CalculateTaxes_Case4_ShouldReturnCorrectTaxes()
        {
            // Arrange - Case #4
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 10.00m, 10000),
                new StockOperation(OperationType.Buy, 25.00m, 5000),
                new StockOperation(OperationType.Sell, 15.00m, 10000)
            };

            // Act
            var results = _taxCalculationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0); // Buy operation - no tax
            results[1].Tax.Should().Be(0); // Buy operation - no tax
            results[2].Tax.Should().Be(0); // Sell at weighted average price - no gain/loss, no tax
        }

        [Fact]
        public void CalculateTaxes_Case9_ShouldReturnCorrectTaxes()
        {
            // Arrange - Case #9
            var operations = new List<StockOperation>
            {
                new StockOperation(OperationType.Buy, 5000.00m, 10),
                new StockOperation(OperationType.Sell, 4000.00m, 5),
                new StockOperation(OperationType.Buy, 15000.00m, 5),
                new StockOperation(OperationType.Buy, 4000.00m, 2),
                new StockOperation(OperationType.Buy, 23000.00m, 2),
                new StockOperation(OperationType.Sell, 20000.00m, 1),
                new StockOperation(OperationType.Sell, 12000.00m, 10),
                new StockOperation(OperationType.Sell, 15000.00m, 3)
            };

            // Act
            var results = _taxCalculationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(8);
            results[0].Tax.Should().Be(0); // Buy operation - no tax
            results[1].Tax.Should().Be(0); // Loss of R$5,000 - no tax
            results[2].Tax.Should().Be(0); // Buy operation - no tax
            results[3].Tax.Should().Be(0); // Buy operation - no tax
            results[4].Tax.Should().Be(0); // Buy operation - no tax
            results[5].Tax.Should().Be(0); // Sell below threshold - no tax
            results[6].Tax.Should().Be(1000.00m); // Profit of R$10,000, offset by R$5,000 loss, tax on R$5,000 = R$1,000
            results[7].Tax.Should().Be(2400.00m); // Profit of R$12,000, 20% tax = R$2,400
        }
    }
}