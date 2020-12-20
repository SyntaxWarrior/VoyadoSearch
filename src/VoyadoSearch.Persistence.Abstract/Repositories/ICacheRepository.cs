using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence.Abstract.Entities;

namespace VoyadoSearch.Persistence.Abstract.Repositories
{
    public interface ICacheRepository
    {
        Task<List<SearchCacheRepositoryDto>> Search(string[] terms, string[] engineIds, CancellationToken ct);
        void Add(string engineId, string term, long hits);
        Task SaveChangesAsync(CancellationToken ct);
    }
}