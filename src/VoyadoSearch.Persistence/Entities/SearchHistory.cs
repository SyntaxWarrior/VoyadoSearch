using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoyadoSearch.Persistence.Entities
{
    /// <summary>
    /// This table will store search history to present to the user so they can redo a search easily.
    /// </summary>
    [Table("History", Schema = "search")]
    public class SearchHistory: TimeTrackBase
    {
        public Guid Id { get; set; }
        public string  Term { get; set; }
    }
}
