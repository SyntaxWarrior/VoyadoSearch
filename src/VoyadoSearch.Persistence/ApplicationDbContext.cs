using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence.Entities;

namespace VoyadoSearch.Persistence
{
    /// <summary>
    /// The database context for Voyado Search.
    /// 
    /// Code first Database context. 
    /// Use: add-migration name-of-migration-here in PM console to add new migrations with this project as the default project
    /// These migrations will then be applied on project start.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<SearchCache> SearchCache { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        /// <summary>
        /// Save changes to the database and updates timestamps on entities.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            PreSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Update timestamp fields on all entities that inherit from TimeTrackBase class (should be all of them)
        /// so that we dont have to worry about updating dates on rows in every operation.
        /// </summary>
        private void PreSaveChanges()
        {
            foreach (var history in ChangeTracker
                .Entries()
                .Where(e => e.Entity is TimeTrackBase && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as TimeTrackBase))
            {
                history.Updated = DateTimeOffset.UtcNow;

                if (history.Created == null)
                {
                    history.Created = DateTimeOffset.UtcNow;
                }
            }
        }

        // whenever we create a new database, make sure we add our indexes.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            AddIndexes(builder);
            base.OnModelCreating(builder);
        }

        // add indexes to the columns that we will be continously searching over to present historic values of searches.
        private static void AddIndexes(ModelBuilder builder)
        {
            builder.Entity<SearchCache>()
                .HasIndex(p => p.Term)
                .HasFilter("[Term] IS NOT NULL")
                .HasDatabaseName("IX_SearchCache_Term");

            builder.Entity<SearchCache>()
                .HasIndex(p => p.Created)
                .HasDatabaseName("IX_SearchCache_Created");

            builder.Entity<SearchCache>()
                .HasIndex(p => p.EngineId)
                .HasDatabaseName("IX_SearchCache_EngineId");
        }

        public void MigrateDatabase(ILogger logger)
        {
            var migrationNames = Database.GetPendingMigrations();

            if (migrationNames.Any())
            {
                logger.LogInformation("Applying {migrations} migrations to database.", migrationNames.Count());
                foreach (var migrationName in migrationNames)
                {
                    logger.LogInformation("Migration to apply: [" + migrationName + "].");
                }

                Database.Migrate();
            }
            else
            {
                logger.LogInformation("No migrations to apply.");
            }
        }
    }
}
