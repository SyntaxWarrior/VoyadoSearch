using System;

namespace VoyadoSearch.Persistence.Abstract.Entities
{
    public class SearchHistoryRepositoryDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset SearchDate { get; set; }
        public string  Term { get; set; }

        public SearchHistoryRepositoryDto(Guid id, DateTimeOffset searchDate, string term)
        {
            Id = id;
            SearchDate = searchDate;
            Term = term;
        }
    }
}
