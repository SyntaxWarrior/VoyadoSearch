using System.Collections.Generic;

namespace VoyadoSearch.Domain.Abstract.DTO
{
    public class SearchResultDomainDto
    {
        public IEnumerable<SearchEngineResultDomainDto> Results { get; }

        public SearchResultDomainDto(IEnumerable<SearchEngineResultDomainDto> engineResults)
        {
            Results = engineResults;
        }
    }
}