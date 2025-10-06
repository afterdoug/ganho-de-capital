# Domain Tests for GanhoDeCapital

This directory contains unit tests for the domain layer of the GanhoDeCapital application.

## Test Structure

The tests are organized to match the structure of the domain layer:

- **Entities**: Tests for domain entities like Position, StockOperation, and TaxCalculationResult
- **Services**: Tests for domain services like TaxRateCalculator and TaxCalculationService
- **Specifications**: Tests for domain specifications like LossOperationSpecification, ProfitableOperationSpecification, and TaxExemptOperationSpecification
- **Strategies**: Tests for operation strategies like BuyOperationStrategy
- **Providers**: Tests for providers like BrazilianTaxRulesProvider

## Testing Approach

These tests use:
- **xUnit** as the testing framework
- **FluentAssertions** for more readable assertions
- **NSubstitute** for mocking dependencies where needed

## Notes

- Some tests use reflection to access private setters for testing purposes
- The TaxCalculationService tests are limited since we don't have the actual implementation in the provided code
- Tests focus on verifying the behavior of individual domain components in isolation