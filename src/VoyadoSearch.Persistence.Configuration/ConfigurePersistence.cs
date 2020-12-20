using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VoyadoSearch.Persistence;
using VoyadoSearch.Persistence.Abstract.Repositories;
using VoyadoSearch.Persistence.Repositories;

namespace VoyagoSearch.Persistence.Configuration
{
    public static class ConfigurePersistence
    {
        /// <summary>
        /// Setup persistance connection to underlying database
        /// </summary>
        /// <param name="services">Application IServiceCollection instance</param>
        /// <param name="configuration">Application IConfiguration instance</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString, sqlOptions => {});
                    options.EnableSensitiveDataLogging(true);
                },
                ServiceLifetime.Transient
            );

            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddTransient<ISearchRepository, SearchRepository>();

            return services;
        }

        /// <summary>
        /// When called checks if there are any pending migrations to apply and updages the database as neccessary.
        /// </summary>
        /// <param name="app">Application instance of IApplicationBuilder</param>
        /// <param name="scopeFactory">The IServiceScopeFactory instance of the application</param>
        /// <param name="logger">Reference to ILogger instance</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app, IServiceScopeFactory scopeFactory, ILogger logger)
        {
            logger.LogInformation("Migration start.");
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                db.MigrateDatabase(logger);
            }
            logger.LogInformation("Migrations complete.");

            return app;
        }
    }
}
