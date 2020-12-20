using System.Collections.Generic;

namespace VoyadoSearch.Domain.Abstract.DTO
{
    public class SearchEngineResultDomainDto
    {
        public string EngineId { get; }
        public string DisplayName { get; }
        public string SearchTerm { get; }
        public long HitCount { get; }

        public SearchEngineResultDomainDto(string engineId, string displayName, long hits, string searchTerm)
        {
            EngineId = engineId;
            DisplayName = displayName;
            HitCount = hits;
            SearchTerm = searchTerm;
        }
    }
}
