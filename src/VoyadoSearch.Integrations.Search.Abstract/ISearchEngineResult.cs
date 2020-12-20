namespace VoyadoSearch.Integraions.Search.Abstract
{
    public interface ISearchEngineResult
    {
        string EngineId { get; }
        long Hits { get; }
    }
}