using System.Threading;
using System.Threading.Tasks;

namespace VoyadoSearch.Integraions.Search.Abstract
{
    /// <summary>
    /// This interface will be implemented by all search engines. 
    /// This way I can simply add another search engine by implementing this interface
    /// And the processes above will implement them automatically (when they are added to the DI)
    /// This solution is based on this SO Answer which I though would fit this case well. 
    /// https://stackoverflow.com/a/52122571/937131
    /// Summarised: if multiple implementations of an interface exist, you can
    /// get them all injected as an enumerable, which suits this case perfectly 
    /// </summary>
    public interface ISearchEngine
    {
        /// <summary>
        /// Unique Id of this search Engine
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Display name for user
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Search method that will search for the sentence entered as one term.
        /// Request this once for each term to be searched for
        /// </summary>
        /// <param name="searchTerm">the search term to search for</param>
        /// <returns>A result of the search</returns>
        Task<ISearchEngineResult> Search(string searchTerm, CancellationToken ct);
    }
}
