using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GanhoDeCapital.Application.DTOs;
using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GanhoDeCapital.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Setup dependency injection
            using var host = CreateHostBuilder(args).Build();
            
            // Get the tax calculation service from DI container
            var taxCalculationService = host.Services.GetRequiredService<ITaxCalculationApplicationService>();
            
            // Process input lines
            string? line;
            while ((line = Console.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
            {
                try
                {
                    // Deserialize the JSON input
                    var operations = JsonSerializer.Deserialize<List<StockOperationDto>>(line, 
                        new JsonSerializerOptions 
                        { 
                            PropertyNameCaseInsensitive = true
                        });
                    
                    if (operations == null)
                    {
                        Console.Error.WriteLine("Failed to parse input JSON");
                        continue;
                    }
                    
                    // Calculate taxes - each line is processed independently
                    var taxResults = taxCalculationService.CalculateTaxes(operations);
                    
                    // Serialize and output the results
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
            
            await host.StopAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
                });
    }
}