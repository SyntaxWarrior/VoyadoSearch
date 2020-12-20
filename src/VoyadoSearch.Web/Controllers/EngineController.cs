using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VoyadoSearch.Contracts;
using VoyadoSearch.Domain.Abstract.Processes;

namespace VoyagoSearch.Web.Controllers
{
    [ApiController]
    [Route("engine")]
    public class EngineController : ControllerBase
    {
        private readonly ISearchEngineListProcess _listSearchEnginesProcess;

        public EngineController(
            ISearchEngineListProcess listSearchEnginesProcess)
        {
            _listSearchEnginesProcess = listSearchEnginesProcess;
        }

        /// <summary>
        /// Get a list of available search engines that can be used in the search request.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>A list of usable search engines</returns>
        [HttpGet("list")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public ActionResult<IEnumerable<SearchEngineContract>> GetSearchEngines(CancellationToken ct)
        {
            var engines = _listSearchEnginesProcess.List();
            var contract = engines.Select(e => new SearchEngineContract(e.Id, e.DisplayName));
            return Ok(contract);
        }
    }
}
