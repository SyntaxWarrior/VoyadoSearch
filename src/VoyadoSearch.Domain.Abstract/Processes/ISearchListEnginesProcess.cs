using System.Threading;
using VoyadoSearch.Domain.Abstract.DTO;

namespace VoyadoSearch.Domain.Abstract.Processes
{
    public interface ISearchEngineListProcess
    {
        /// <summary>
        /// Lists all available search engines
        /// </summary>
        /// <returns>a list of the search engines that can be used</returns>
        SearchEngineDomainDto[] List();
    }
}