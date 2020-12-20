using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence.Abstract.Entities;
using VoyadoSearch.Persistence.Abstract.Repositories;
using VoyadoSearch.Persistence.Entities;

namespace VoyadoSearch.Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly int _cacheLifeTimeDays;

        public CacheRepository(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _cacheLifeTimeDays = int.Parse(configuration.GetSection("Cache:CacheLifeTimeDays").Value);
        }

        /// <summary>
        /// Search for cache matches to a previous query
        /// </summary>
        /// <param name="terms">A list of terms to get cache results for</param>
        /// <param name="engineIds">The engines we want cache results from</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>A list of cache matches</returns>
        public Task<List<SearchCacheRepositoryDto>> Search(string[] terms, string[] engineIds, CancellationToken ct)
        {
            var oldestAllowedCache = DateTimeOffset.UtcNow.AddDays(-_cacheLifeTimeDays);

            // indexes have been created on these columns in ApplicationDbContext
            var cache = _context.SearchCache.Where(s =>
                    s.Created >= oldestAllowedCache
                    && engineIds.Contains(s.EngineId)
                    && terms.Contains(s.Term)
                )
                .OrderByDescending(c => c.Created)
                .Select(c => new SearchCacheRepositoryDto(c.Id, c.EngineId, c.Term, c.Hits))
                .ToListAsync();

            return cache;
        }

        /// <summary>
        /// Add a new cache value to the database 
        /// </summary>
        /// <param name="engineId"></param>
        /// <param name="term"></param>
        /// <param name="hits"></param>
        public void Add(string engineId, string term, long hits)
        {
            _context.SearchCache.Add(new SearchCache()
            {
                EngineId = engineId,
                Term = term,
                Hits = hits
            });
        }

        /// <summary>
        /// No changes (or Additions) are stored untill you save the changes.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
