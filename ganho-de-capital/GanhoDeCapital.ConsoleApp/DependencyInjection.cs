using GanhoDeCapital.Application.Services;
using GanhoDeCapital.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GanhoDeCapital.ConsoleApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITaxCalculationService, TaxCalculationService>();
            services.AddScoped<ITaxCalculationApplicationService, TaxCalculationApplicationService>();
            
            return services;
        }
    }
}