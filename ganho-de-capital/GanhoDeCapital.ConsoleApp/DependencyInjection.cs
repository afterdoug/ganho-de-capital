using System.Collections.Generic;
using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Domain.Entities;
using GanhoDeCapital.Domain.Services;
using GanhoDeCapital.Domain.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace GanhoDeCapital.ConsoleApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register strategies
            services.AddSingleton<BuyOperationStrategy>();
            services.AddSingleton<SellOperationStrategy>();
            
            // Register strategy dictionary
            services.AddSingleton(provider => 
            {
                return new Dictionary<OperationType, IOperationStrategy>
                {
                    { OperationType.Buy, provider.GetRequiredService<BuyOperationStrategy>() },
                    { OperationType.Sell, provider.GetRequiredService<SellOperationStrategy>() }
                };
            });
            
            // Register services
            services.AddScoped<ITaxCalculationService>(provider => 
            {
                var strategies = provider.GetRequiredService<Dictionary<OperationType, IOperationStrategy>>();
                return new TaxCalculationService(strategies);
            });
            
            services.AddScoped<ITaxCalculationApplicationService, TaxCalculationApplicationService>();
            
            return services;
        }
    }
}