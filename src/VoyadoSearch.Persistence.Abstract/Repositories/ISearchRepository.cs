using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence.Abstract.Entities;

namespace VoyadoSearch.Persistence.Abstract.Repositories
{
    public interface ISearchRepository
    {
        void Add(string term);
        Task<List<SearchHistoryRepositoryDto>> GetHistoric(int count, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}