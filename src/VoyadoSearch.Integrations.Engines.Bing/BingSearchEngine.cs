using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VoyadoSearch.Integraions.Search.Abstract;
using System.Threading;

namespace VoyadoSearch.Integrations.Engines.Bing
{
    public class BingSearchEngine : ISearchEngine
    {
        public string Id => "bing";

        public string DisplayName => "Bing";

        private readonly string _baseQuery;
        private readonly string _key;

        private readonly ILogger<BingSearchEngine> _logger;

        /// <summary>
        /// Implementation that searches bing for a match count.
        /// </summary>
        public BingSearchEngine(ILogger<BingSearchEngine> logger, IConfiguration configuration)
        {
            _baseQuery = configuration.GetSection("SearchEngines:Bing:Query").Value;
            _key = configuration.GetSection("SearchEngines:Bing:Key").Value;
            _logger = logger;
        }

        // example of usage from here
        // https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/quickstarts/csharp
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
                    long matchCount = Convert.ToInt64(data.webPages.totalEstimatedMatches);

                    return new SearchEngineResult(this.Id, matchCount);
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("Could not parse Bing search result in BingSearchEngine.Search", e);
                }
            }
            else
            {
                _logger.LogError("Failed to query Bing search engine. Query returned code: " + responseCode.ToString());
                return new SearchEngineResult(this.Id, 0);
            }
        }

        private async Task<WebResponse> MakeQueryRequest(string searchTerm, CancellationToken ct)
        {
            var uriQuery = _baseQuery.Replace("{query}", Uri.EscapeDataString(searchTerm));

            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = _key;

            var response = await request.GetResponseAsync();
            return response;
        }
    }
}
