using Microsoft.EntityFrameworkCore;
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
    public class SearchRepository : ISearchRepository
    {
        private readonly IApplicationDbContext _context;

        public SearchRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of historic searches that have been performaed
        /// </summary>
        /// <param name="count">The number of previous searches to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A list of previous searches</returns>
        public Task<List<SearchHistoryRepositoryDto>> GetHistoric(int count, CancellationToken ct)
        {
            return _context.SearchHistory
                .OrderByDescending(c => c.Created)
                .Select(h => new SearchHistoryRepositoryDto(h.Id, h.Created ?? DateTimeOffset.MinValue, h.Term))
                .Take(count)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Add a new search to the history
        /// </summary>
        /// <param name="term">The term that was searched for</param>
        public void Add(string term)
        {
            _context.SearchHistory.Add(new SearchHistory()
            {
                Term = term
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
