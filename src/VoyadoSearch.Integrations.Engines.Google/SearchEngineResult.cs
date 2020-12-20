using VoyadoSearch.Integraions.Search.Abstract;

namespace VoyadoSearch.Integrations.Engines.Google
{
    public class SearchEngineResult : ISearchEngineResult
    {
        public string EngineId { get; }
        public long Hits { get; }

        public SearchEngineResult(string engineId, long hits)
        {
            EngineId = engineId;
            Hits = hits;
        }
    }
}
