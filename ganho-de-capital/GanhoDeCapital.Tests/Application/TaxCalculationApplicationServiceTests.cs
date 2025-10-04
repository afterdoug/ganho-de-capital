using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;
using NSubstitute;
using Xunit;

namespace GanhoDeCapital.Tests.Application
{
    public class TaxCalculationApplicationServiceTests
    {
        private readonly ITaxCalculationService _taxCalculationService;
        private readonly TaxCalculationApplicationService _applicationService;

        public TaxCalculationApplicationServiceTests()
        {
            // Setup mock for domain service
            _taxCalculationService = Substitute.For<ITaxCalculationService>();
            _applicationService = new TaxCalculationApplicationService(_taxCalculationService);
        }

        [Fact]
        public void CalculateTaxes_Scenario1_ShouldReturnCorrectTaxes()
        {
            // Arrange
            var operationDtos = new List<StockOperationDto>
            {
                new StockOperationDto { Operation = "buy", UnitCost = 10.00m, Quantity = 100 },
                new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 },
                new StockOperationDto { Operation = "sell", UnitCost = 15.00m, Quantity = 50 }
            };

            // Setup mock to return expected domain results
            _taxCalculationService
                .CalculateTaxes(Arg.Any<IEnumerable<StockOperation>>())
                .Returns(new List<TaxCalculationResult>
                {
                    new TaxCalculationResult(0),
                    new TaxCalculationResult(0),
                    new TaxCalculationResult(0)
                });

            // Act
            var results = _applicationService.CalculateTaxes(operationDtos).ToList();

            // Assert
            results.Should().HaveCount(3);
            results[0].Tax.Should().Be(0);
            results[1].Tax.Should().Be(0);
            results[2].Tax.Should().Be(0);
        }
    }
}