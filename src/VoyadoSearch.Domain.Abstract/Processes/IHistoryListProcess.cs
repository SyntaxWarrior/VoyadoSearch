using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Domain.Abstract.DTO;

namespace VoyadoSearch.Domain.Abstract.Processes
{
    public interface IHistoryListProcess
    {
        /// <summary>
        /// Get a list of historic searches that have been made
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>A list of historic searches</returns>
        Task<List<SearchHistoryDomainDto>> Get(CancellationToken ct);
    }
}