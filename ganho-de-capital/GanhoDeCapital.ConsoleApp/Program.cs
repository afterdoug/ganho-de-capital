using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GanhoDeCapital.ConsoleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        var taxCalculationService = host.Services.GetRequiredService<ITaxCalculationApplicationService>();

        ProcessInputLines(taxCalculationService);

        await host.StopAsync();
    }

    private static void ProcessInputLines(ITaxCalculationApplicationService taxCalculationService)
    {
        string? line;
        while ((line = Console.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
        {
            try
            {
                List<StockOperationDto>? operations = ParseToOperations(line);

                if (operations == null)
                {
                    Console.Error.WriteLine("Failed to parse input JSON");
                    continue;
                }

                var taxResults = taxCalculationService.CalculateTaxes(operations);

                var jsonOutput = JsonSerializer.Serialize(taxResults);
                Console.WriteLine(jsonOutput);
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Error parsing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing input: {ex.Message}");
            }
        }
    }

    private static List<StockOperationDto>? ParseToOperations(string line)
    {
        return JsonSerializer.Deserialize<List<StockOperationDto>>(line, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddApplicationServices();
            });
}