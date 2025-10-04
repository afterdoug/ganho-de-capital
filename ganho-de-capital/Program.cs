using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GanhoDeCapital
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            Console.WriteLine("Lançamento de operação:");
            while ((line = Console.ReadLine()) != null)
            {
                try
                {
                    var operations = JsonSerializer.Deserialize<List<StockOperation>>(line, 
                        new JsonSerializerOptions 
                        { 
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    
                    var taxCalculator = new TaxCalculator();
                    var taxResult = taxCalculator.CalculateTax(operations);
                    
                    Console.WriteLine(JsonSerializer.Serialize(taxResult));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing input: {ex.Message}");
                }
            }
        }
    }
}