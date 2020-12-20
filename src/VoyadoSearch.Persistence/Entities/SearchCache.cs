using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoyadoSearch.Persistence.Entities
{
    [Table("Search", Schema = "cache")]
    public class SearchCache: TimeTrackBase
    {
        public Guid Id { get; set; }
        public string EngineId { get; set; }
        public string Term { get; set; }
        public long Hits { get; set; }
    }
}
