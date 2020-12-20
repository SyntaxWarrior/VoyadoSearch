using System;

namespace VoyadoSearch.Persistence.Abstract.Entities
{
    public class SearchCacheRepositoryDto
    {
        public Guid Id { get; set; }
        public string EngineId { get; set; }
        public string Term { get; set; }
        public long Hits { get; set; }

        public SearchCacheRepositoryDto(Guid id, string engineId, string term, long hits)
        {
            Id = id;
            EngineId = engineId;
            Term = term;
            Hits = hits;
        }
    }
}
