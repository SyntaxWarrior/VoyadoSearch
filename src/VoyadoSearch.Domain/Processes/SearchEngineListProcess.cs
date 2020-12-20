using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VoyadoSearch.Domain.Abstract.DTO;
using VoyadoSearch.Domain.Abstract.Processes;
using VoyadoSearch.Integraions.Search.Abstract;

namespace VoyadoSearch.Domain.Processes
{
    public class SearchEngineListProcess : ISearchEngineListProcess
    {
        private readonly IEnumerable<ISearchEngine> _engines;

        public SearchEngineListProcess(IEnumerable<ISearchEngine> engines)
        {
            _engines = engines;
        }

        /// <summary>
        /// Lists all available search engines
        /// </summary>
        /// <returns>a list of the search engines that can be used</returns>
        public SearchEngineDomainDto[] List()
        {
            return _engines.Select(e =>
                new SearchEngineDomainDto(e.Id, e.DisplayName)
            ).ToArray();
        }
    }
}