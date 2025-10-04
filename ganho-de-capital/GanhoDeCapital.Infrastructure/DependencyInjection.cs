using GanhoDeCapital.Domain.Services;
using GanhoDeCapital.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GanhoDeCapital.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ITaxRulesProvider, BrazilianTaxRulesProvider>();
            
            return services;
        }
    }
}