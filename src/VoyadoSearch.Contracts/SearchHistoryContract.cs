using System;

namespace VoyadoSearch.Contracts
{
    public class SearchHistoryContract
    {
        public DateTimeOffset SearchDate { get; }
        public string Term { get; }

        public SearchHistoryContract(DateTimeOffset searchDate, string term)
        {
            SearchDate = searchDate;
            Term = term;
        }
    }
}
