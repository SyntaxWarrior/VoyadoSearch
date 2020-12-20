using Microsoft.Extensions.DependencyInjection;
using VoyadoSearch.Integraions.Search.Abstract;
using VoyadoSearch.Integrations.Engines.Bing;
using VoyadoSearch.Integrations.Engines.Google;

namespace VoyadoSearch.Integrations.Search.Configuration
{
    public static class Configuration
    {
        public static IServiceCollection AddSearchIntegrations(this IServiceCollection services)
        {
            services.AddScoped<ISearchEngine, BingSearchEngine>();
            services.AddScoped<ISearchEngine, GoogleSearchEngine>();

            return services;
        }
    }
}
