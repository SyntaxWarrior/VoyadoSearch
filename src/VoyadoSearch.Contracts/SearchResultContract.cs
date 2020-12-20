using System.Collections.Generic;

namespace VoyadoSearch.Contracts
{
    public class SearchResultContract
    {
        public long TotalHits { get; }
        public long ElapsedMilliseconds { get; }
        public IEnumerable<SearchEngineResultContract> Results { get; }

        public SearchResultContract(long totalHits, long elapsedMilliseconds, IEnumerable<SearchEngineResultContract> engineResults)
        {
            TotalHits = totalHits;
            ElapsedMilliseconds = elapsedMilliseconds;
            Results = engineResults;
        }
    }
}