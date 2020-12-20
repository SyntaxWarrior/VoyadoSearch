using System;

namespace VoyadoSearch.Domain.Abstract.DTO
{
    public class SearchHistoryDomainDto
    {
        public DateTimeOffset SearchDate { get; }
        public string Term { get; }

        public SearchHistoryDomainDto(DateTimeOffset searchDate, string term)
        {
            SearchDate = searchDate;
            Term = term;
        }
    }
}
