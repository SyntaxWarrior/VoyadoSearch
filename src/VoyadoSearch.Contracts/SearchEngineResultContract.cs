namespace VoyadoSearch.Contracts
{
    public class SearchEngineResultContract
    {
        public string EngineId { get; }
        public string DisplayName { get; }
        public string SearchTerm { get; }
        public long HitCount { get; }

        public SearchEngineResultContract(string engineId, string displayName, long hits, string searchTerm)
        {
            EngineId = engineId;
            DisplayName = displayName;
            HitCount = hits;
            SearchTerm = searchTerm;
        }
    }
}
