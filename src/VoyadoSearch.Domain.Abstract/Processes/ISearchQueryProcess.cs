using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Domain.Abstract.DTO;

namespace VoyadoSearch.Domain.Abstract.Processes
{
    public interface ISearchQueryProcess
    {
        /// <summary>
        /// Search for the given terms in all requested search engines.
        /// </summary>
        /// <param name="searchEngines">string[] of search engine ids to use</param>
        /// <param name="searchTerms">The search query</param>
        /// <returns>A list of matches and hits</returns>
        Task<SearchResultDomainDto> Search(string[] searchEngines, string searchTerms, CancellationToken ct);
    }
}