using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Domain.Abstract.DTO;
using VoyadoSearch.Domain.Abstract.Processes;
using VoyadoSearch.Persistence.Abstract.Repositories;

namespace VoyadoSearch.Domain.Processes
{
    public class HistoryListProcess : IHistoryListProcess
    {
        private readonly ISearchRepository _searchRepository;

        public HistoryListProcess(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        /// <summary>
        /// Get a list of historic searches that have been made
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>A list of historic searches</returns>
        public async Task<List<SearchHistoryDomainDto>> Get(CancellationToken ct)
        {
            var result = await _searchRepository
                .GetHistoric(9, ct);

            return result
                .Select(h => new SearchHistoryDomainDto(h.SearchDate, h.Term))
                .ToList();
        }
    }
}
