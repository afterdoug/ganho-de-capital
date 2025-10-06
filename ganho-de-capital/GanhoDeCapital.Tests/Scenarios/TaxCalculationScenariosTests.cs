using FluentAssertions;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GanhoDeCapital.Tests.Scenarios;

public class TaxCalculationScenariosTests
{
    private readonly ITaxCalculationApplicationService _applicationService;

    public TaxCalculationScenariosTests()
    {
        // Setup real implementations for scenario testing
        ITaxCalculationService taxCalculationService = new TaxCalculationService();
        _applicationService = new TaxCalculationApplicationService(taxCalculationService);
    }

    [Fact]
    public void Case1_BuyAndSellBelowThreshold()
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
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Sell below threshold - no tax
        results[2].Tax.Should().Be(0); // Sell below threshold - no tax
    }

    [Fact]
    public void Case2_ProfitAndLoss()
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
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(10000.00m); // Profit of R$50,000, 20% tax = R$10,000
        results[2].Tax.Should().Be(0); // Loss of R$25,000 - no tax
    }

    [Fact]
    public void Case1And2_IndependentSimulations()
    {
        // Arrange - Case #1 + Case #2
        var operations1 = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 100 },
            new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 },
            new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 }
        };

        var operations2 = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 5.00m, Quantity = 5000 }
        };

        // Act
        var results1 = _applicationService.CalculateTaxes(operations1).ToList();
        var results2 = _applicationService.CalculateTaxes(operations2).ToList();

        // Assert
        results1.Should().HaveCount(3);
        results1[0].Tax.Should().Be(0);
        results1[1].Tax.Should().Be(0);
        results1[2].Tax.Should().Be(0);

        results2.Should().HaveCount(3);
        results2[0].Tax.Should().Be(0);
        results2[1].Tax.Should().Be(10000.00m);
        results2[2].Tax.Should().Be(0);
    }

    [Fact]
    public void Case3_LossOffsetAgainstFutureProfit()
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
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Loss of R$25,000 - no tax
        results[2].Tax.Should().Be(1000.00m); // Profit of R$30,000, offset by R$25,000 loss, tax on R$5,000 = R$1,000
    }

    [Fact]
    public void Case4_WeightedAverageWithNoGainOrLoss()
    {
        // Arrange - Case #4
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "buy", UnitCost = 25.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 10000 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

        // Assert
        results.Should().HaveCount(3);
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Buy operation - no tax
        results[2].Tax.Should().Be(0); // Sell at weighted average price - no gain/loss, no tax
    }

    [Fact]
    public void Case5_WeightedAverageWithProfit()
    {
        // Arrange - Case #5
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "buy", UnitCost = 25.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 25.00m, Quantity = 5000 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

        // Assert
        results.Should().HaveCount(4);
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Buy operation - no tax
        results[2].Tax.Should().Be(0); // Sell at weighted average price - no gain/loss, no tax
        results[3].Tax.Should().Be(10000.00m); // Profit of R$50,000, 20% tax = R$10,000
    }

    [Fact]
    public void Case6_ComplexLossOffsetScenario()
    {
        // Arrange - Case #6
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 2.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new StockOperationDto { Operation = "sell", UnitCost = 25.00m, Quantity = 1000 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

        // Assert
        results.Should().HaveCount(5);
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Loss of R$40,000 - no tax
        results[2].Tax.Should().Be(0); // Profit of R$20,000, offset by previous loss - no tax
        results[3].Tax.Should().Be(0); // Profit of R$20,000, offset by remaining loss - no tax
        results[4].Tax.Should().Be(3000.00m); // Profit of R$15,000, no loss to offset, 20% tax = R$3,000
    }

    [Fact]
    public void Case7_ExtendedComplexScenario()
    {
        // Arrange - Case #7
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 2.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new StockOperationDto { Operation = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new StockOperationDto { Operation = "sell", UnitCost = 25.00m, Quantity = 1000 },
            new StockOperationDto { Operation = "buy", UnitCost = 20.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 5000 },
            new StockOperationDto { Operation = "sell", UnitCost = 30.00m, Quantity = 4350 },
            new StockOperationDto { Operation = "sell", UnitCost = 30.00m, Quantity = 650 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

        // Assert
        results.Should().HaveCount(9);
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(0); // Loss of R$40,000 - no tax
        results[2].Tax.Should().Be(0); // Profit of R$20,000, offset by previous loss - no tax
        results[3].Tax.Should().Be(0); // Profit of R$20,000, offset by remaining loss - no tax
        results[4].Tax.Should().Be(3000.00m); // Profit of R$15,000, no loss to offset, 20% tax = R$3,000
        results[5].Tax.Should().Be(0); // Buy operation - no tax
        results[6].Tax.Should().Be(0); // Loss of R$25,000 - no tax
        results[7].Tax.Should().Be(3700.00m); // Profit of R$43,500, offset by R$25,000 loss, tax on R$18,500 = R$3,700
        results[8].Tax.Should().Be(0); // Profit of R$6,500, below threshold - no tax
    }

    [Fact]
    public void Case8_HighValueOperations()
    {
        // Arrange - Case #8
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 50.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "buy", UnitCost = 20.00m, Quantity = 10000 },
            new StockOperationDto { Operation = "sell", UnitCost = 50.00m, Quantity = 10000 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

        // Assert
        results.Should().HaveCount(4);
        results[0].Tax.Should().Be(0); // Buy operation - no tax
        results[1].Tax.Should().Be(80000.00m); // Profit of R$400,000, 20% tax = R$80,000
        results[2].Tax.Should().Be(0); // Buy operation - no tax
        results[3].Tax.Should().Be(60000.00m); // Profit of R$300,000, 20% tax = R$60,000
    }

    [Fact]
    public void Case9_ComplexWeightedAverageAndLossOffset()
    {
        // Arrange - Case #9
        var operations = new List<StockOperationDto>
        {
            new StockOperationDto { Operation = "buy", UnitCost = 5000.00m, Quantity = 10 },
            new StockOperationDto { Operation = "sell", UnitCost = 4000.00m, Quantity = 5 },
            new StockOperationDto { Operation = "buy", UnitCost = 15000.00m, Quantity = 5 },
            new StockOperationDto { Operation = "buy", UnitCost = 4000.00m, Quantity = 2 },
            new StockOperationDto { Operation = "buy", UnitCost = 23000.00m, Quantity = 2 },
            new StockOperationDto { Operation = "sell", UnitCost = 20000.00m, Quantity = 1 },
            new StockOperationDto { Operation = "sell", UnitCost = 12000.00m, Quantity = 10 },
            new StockOperationDto { Operation = "sell", UnitCost = 15000.00m, Quantity = 3 }
        };

        // Act
        var results = _applicationService.CalculateTaxes(operations).ToList();

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