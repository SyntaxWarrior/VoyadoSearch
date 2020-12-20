using System.Threading.Tasks;
using System.Collections.Generic;
using VoyadoSearch.Integraions.Search.Abstract;
using System.Threading;
using VoyadoSearch.Persistence.Abstract.Entities;
using VoyadoSearch.Domain.Abstract.DTO;
using System.Linq;
using VoyadoSearch.Domain.Abstract.Processes;
using Microsoft.Extensions.Logging;
using VoyadoSearch.Persistence.Abstract.Repositories;

namespace VoyadoSearch.Domain.Processes
{
    public class SearchQueryProcess : ISearchQueryProcess
    {
        private readonly ILogger<SearchQueryProcess> _logger;
        private readonly IEnumerable<ISearchEngine> _engines;
        private readonly ICacheRepository _cacheRepository;
        private readonly ISearchRepository _searchRepository;

        public SearchQueryProcess(
            ILogger<SearchQueryProcess> logger,
            IEnumerable<ISearchEngine> engines,
            ICacheRepository cacheRepository,
            ISearchRepository searchRepository)
        {
            _logger = logger;
            _engines = engines;
            _cacheRepository = cacheRepository;
            _searchRepository = searchRepository;
        }

        /// <summary>
        /// Search for the given terms in all requested search engines.
        /// </summary>
        /// <param name="searchEngines">string[] of search engine ids to use</param>
        /// <param name="searchTerms">The search query</param>
        /// <returns>A list of matches and hits</returns>
        public async Task<SearchResultDomainDto> Search(string[] searchEngines, string searchTerms, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(searchTerms))
                return new SearchResultDomainDto(new List<SearchEngineResultDomainDto>());

            _searchRepository.Add(searchTerms);
            await _searchRepository.SaveChangesAsync(ct);

            // the search should be conducted on each word separately. 
            // Therefore we split it up into each word so we can pass 
            // it to the search engines one by one.
            var terms = searchTerms.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            // get a list of caches matching this query.
            var cache = await _cacheRepository.Search(terms, searchEngines, ct);

            var engines = _engines
                .Select(e => new { e.Id, e.DisplayName })
                .ToDictionary(k => k.Id, v => v.DisplayName);
            

            // will keep a list of Tasks for each search that we can await so we do them all in parallel
            var resultListAsync = new List<Task<SearchEngineResultDomainDto>>();

            // iterate over all the engines that are available and query them if they are in the requested searchEngines list.
            // also iterate over every term since we want the result to be for each term separately.
            foreach (var engine in _engines)
            {
                if (searchEngines.Contains(engine.Id))
                {
                    foreach (var term in terms)
                    {
                        resultListAsync.Add(SearchForTerm(engine, term, cache, engines, ct));
                    }
                }
            }

            var resultList = await Task.WhenAll(resultListAsync);

            // save cache values to the database, doing this here after everything is
            // done so we dont get multiple database operations at the same time.
            await _cacheRepository.SaveChangesAsync(ct);

            return new SearchResultDomainDto(resultList);
        }

        /// <summary>
        /// We could just pass the search to each engine right away, but lets try a cache first.
        /// In reality we would probably end up paying for each query, so we might as well reuse
        /// recent results so that we both get a faster response, and a lower cost for the service
        /// and also for the free services we are using they limit requests so lets not hit that limit.
        /// </summary>
        /// <param name="engine">Which search engine to use</param>
        /// <param name="term">The term to search for</param>
        /// <returns>awaitable task with result response</returns>
        private Task<SearchEngineResultDomainDto> SearchForTerm(ISearchEngine engine, string term, List<SearchCacheRepositoryDto> cache, Dictionary<string, string> engines, CancellationToken ct)
        {
            // Each search is run as a separate Task so that we can do them all in parallel.
            // as we need to perform a search for each term in out query this would take some time
            // if we did them all synchronously. Instead, now the total time should be close to the single 
            // slowest call.
            return Task.Run<SearchEngineResultDomainDto>(async () =>
            {
                _logger.LogDebug($"Start search with: {engine.Id} for: {term}");
                
                // check if there is a cached value for this engine and term.
                // This will be an in memory search, but hopefully people arent writing novels in 
                // the query so it should still be fast enough, if not I'll have to rethink this.
                var cacheResult = cache.FirstOrDefault(x => x.EngineId == engine.Id && x.Term == term);
                if (cacheResult != null)
                {
                    _logger.LogDebug($"cache hit for {engine.Id} with: {term}");
                    return new SearchEngineResultDomainDto(cacheResult.EngineId, engines[cacheResult.EngineId], cacheResult.Hits, cacheResult.Term);
                }
                else
                {
                    // if there was no cache, perform a live search, and then cache it.
                    _logger.LogDebug($"performing search on: {engine.Id} for: {term}");
                    var searchResult = await engine.Search(term, ct);

                    var result = new SearchEngineResultDomainDto(engine.Id, engines[engine.Id], searchResult.Hits, term);
                    if (result != null)
                    {
                        _cacheRepository.Add(result.EngineId, result.SearchTerm, result.HitCount);
                    }

                    _logger.LogDebug($"{engine.Id} done with: {term}");
                    return result;
                }
            }, ct);
        }
    }
}
