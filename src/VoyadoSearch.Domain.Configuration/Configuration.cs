using Microsoft.Extensions.DependencyInjection;
using VoyadoSearch.Domain.Abstract.Processes;
using VoyadoSearch.Domain.Processes;

namespace VoyadoSearch.Domain.Configuration
{
    public static class Configuration
    {
        public static IServiceCollection AddDomainLogic(this IServiceCollection services)
        {
            services.AddScoped<ISearchEngineListProcess, SearchEngineListProcess>();
            services.AddTransient<ISearchQueryProcess, SearchQueryProcess>();
            services.AddTransient<IHistoryListProcess, HistoryListProcess>();
            return services;
        }
    }
}
