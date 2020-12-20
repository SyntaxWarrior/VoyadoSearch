using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Integraions.Search.Abstract;

namespace VoyadoSearch.Integrations.Engines.Google
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private string _baseQuery;
        private string _key;
        private string _engine;

        private ILogger<GoogleSearchEngine> _logger;

        public string Id => "google";

        public string DisplayName => "Google";

        public GoogleSearchEngine(ILogger<GoogleSearchEngine> logger, IConfiguration configuration)
        {
            _baseQuery = configuration.GetSection("SearchEngines:Google:Query").Value;
            _key = configuration.GetSection("SearchEngines:Google:Key").Value;
            _engine = configuration.GetSection("SearchEngines:Google:EngineId").Value;
            _logger = logger;
        }

        // documentation https://developers.google.com/custom-search/v1/overview
        public async Task<ISearchEngineResult> Search(string searchTerm, CancellationToken ct)
        {
            WebResponse response = await MakeQueryRequest(searchTerm, ct);

            var responseCode = ((HttpWebResponse)response).StatusCode;
            if (responseCode == HttpStatusCode.OK)
            {
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

                try
                {
                    // as dynamic is inherently finiky since its not typed, im wrapping this in a try catch to 
                    // add some information to the exception to make it easier to find potential errors.
                    // one could also return an empty result, but I prefer code that blows up to code that stops
                    // working silently. In a future, this could probably be replaced so that this is logged as an error 
                    // and display in a dashboard instead of taking the service down.

                    // normally I would also probably opt for not using a dynamic approach and instead
                    // having a typed class representing the json object. 
                    // that will have to be a future improvement.
                    dynamic data = JObject.Parse(json);
                    long matchCount = Convert.ToInt64(data.queries.request[0].totalResults);
                    return new SearchEngineResult(this.Id, matchCount);
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("Could not parse Google search result in GoogleSearchEngine.Search", e);
                }
            }
            else
            {
                _logger.LogError("Failed to query Google search engine. Query returned code: " + responseCode.ToString());
                return new SearchEngineResult(this.Id, 0);
            }
        }

        // example query
        // https://www.googleapis.com/customsearch/v1?key=AIzaSyC_rJ8xkYAVjSzGaDPZZ8Zp0rCG2sbqr1A&cx=d57c4a5b910347ebe&q=boskapstorget
        private async Task<WebResponse> MakeQueryRequest(string searchTerm, CancellationToken ct)
        {
            var uriQuery = _baseQuery
                .Replace("{query}", Uri.EscapeDataString(searchTerm))
                .Replace("{key}", _key)
                .Replace("{engineId}", _engine);

            WebRequest request = WebRequest.Create(uriQuery);

            var response = await request.GetResponseAsync();
            return response;
        }
    }
}
