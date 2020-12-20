using System;
using System.Collections.Generic;
using System.Text;

namespace VoyadoSearch.Domain.Abstract.DTO
{
    public class SearchEngineDomainDto
    {
        public string Id { get; }
        public string DisplayName { get; }

        public SearchEngineDomainDto(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}
