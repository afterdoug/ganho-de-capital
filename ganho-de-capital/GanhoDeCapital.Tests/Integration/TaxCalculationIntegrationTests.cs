using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Domain.Services;
using GanhoDeCapital.Infrastructure.Services;
using Xunit;

namespace GanhoDeCapital.Tests.Integration
{
    public class TaxCalculationIntegrationTests
    {
        private readonly ITaxCalculationApplicationService _applicationService;

        public TaxCalculationIntegrationTests()
        {
            // Setup real implementations (not mocks) for integration testing
            ITaxRulesProvider taxRulesProvider = new BrazilianTaxRulesProvider();
            ITaxCalculationService taxCalculationService = new TaxCalculationService(taxRulesProvider);
            _applicationService = new TaxCalculationApplicationService(taxCalculationService);
        }

        [Fact]
        public void Case1_BuyAndSellBelowThreshold_Integration()
        {
            // Arrange - Case #1
            var operations = new List<StockOperationDto>
            {
                new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 100 },
                new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 },
                new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 }
            };

            // Act
            var results = _applicationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0);
            results[1].Tax.Should().Be(0);
            results[2].Tax.Should().Be(0);
        }

        [Fact]
        public void Case2_ProfitAndLoss_Integration()
        {
            // Arrange - Case #2
            var operations = new List<StockOperationDto>
            {
                new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
                new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 5000 },
                new StockOperationDto { Operation = "sell", UnitCost = 5.00m, Quantity = 5000 }
            };

            // Act
            var results = _applicationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0);
            results[1].Tax.Should().Be(10000.00m);
            results[2].Tax.Should().Be(0);
        }

        [Fact]
        public void Case3_LossOffsetAgainstFutureProfit_Integration()
        {
            // Arrange - Case #3
            var operations = new List<StockOperationDto>
            {
                new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
                new StockOperationDto { Operation = "sell", UnitCost = 5.00m, Quantity = 5000 },
                new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 3000 }
            };

            // Act
            var results = _applicationService.CalculateTaxes(operations).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0);
            results[1].Tax.Should().Be(0);
            results[2].Tax.Should().Be(1000.00m);
        }
    }
}