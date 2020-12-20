using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Contracts;
using VoyadoSearch.Domain.Abstract.Processes;

namespace VoyadoSearch.Web.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchQueryProcess _searchQueryProcess;
        private readonly IHistoryListProcess _historyListProcess;

        public SearchController(
            ISearchQueryProcess searchQueryProcess,
            IHistoryListProcess historyListProcess)
        {
            _searchQueryProcess = searchQueryProcess;
            _historyListProcess = historyListProcess;
        }

        /// <summary>
        /// Performs a search of given term on set search engines and returns a result.
        /// </summary>
        /// <param name="enginesToUse">Which engines to search with</param>
        /// <param name="searchTerms">The term to search for</param>
        /// <param name="ct"></param>
        /// <returns>Result of match counts for each requested engine</returns>
        [HttpPost("query")]
        [ResponseCache(NoStore = true)]
        public async Task<ActionResult<SearchResultContract>> Query(string[] enginesToUse, string searchTerms, CancellationToken ct)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await _searchQueryProcess.Search(enginesToUse, searchTerms, ct);

            // if these contracts were more difficult there could be use for automapper but for now this is good enough.
            // no need to complicate it just yet. ;)
            var responseList = result.Results
                .Select(r => new SearchEngineResultContract(r.EngineId, r.DisplayName, r.HitCount, r.SearchTerm))
                .OrderBy(o =>o.DisplayName);

            var totalHits = responseList.Select(r => r.HitCount).Sum();

            stopWatch.Stop();
            var response = new SearchResultContract(totalHits, stopWatch.ElapsedMilliseconds, responseList);

            return Ok(response);
        }

        /// <summary>
        /// Return the latest historic searches
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>A list of previous searches</returns>
        [HttpPost("history")]
        [ResponseCache(Duration = 1)]
        public async Task<ActionResult<List<SearchHistoryContract>>> History(CancellationToken ct)
        {
            var result = await _historyListProcess.Get(ct);

            var response = result
                .Select(r => new SearchHistoryContract(r.SearchDate, r.Term))
                .ToList();

            return Ok(response);
        }
    }
}
