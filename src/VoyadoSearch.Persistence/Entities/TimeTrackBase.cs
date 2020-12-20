using System;

namespace VoyadoSearch.Persistence.Entities
{
    public class TimeTrackBase
    {
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}

