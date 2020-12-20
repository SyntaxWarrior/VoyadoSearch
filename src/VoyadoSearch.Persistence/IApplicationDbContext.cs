using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence.Entities;

namespace VoyadoSearch.Persistence
{
    /// <summary>
    /// This is the only interface which is not in the Abstract project. 
    /// This is because this should not be used by anyone outside of Persistence 
    /// As they should be using the repositories.
    /// </summary>
    public interface IApplicationDbContext
    {
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<SearchCache> SearchCache { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public void MigrateDatabase(ILogger logger);
    }
}