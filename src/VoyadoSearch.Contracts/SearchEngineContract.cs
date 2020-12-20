using System;
using System.Collections.Generic;
using System.Text;

namespace VoyadoSearch.Contracts
{
    public class SearchEngineContract
    {
        public string Id { get; }
        public string DisplayName { get; }

        public SearchEngineContract(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}
