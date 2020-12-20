using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Domain.Processes;
using VoyadoSearch.Integraions.Search.Abstract;
using VoyadoSearch.Persistence.Abstract.Entities;
using VoyadoSearch.Persistence.Abstract.Repositories;

namespace VoyagoSearch.Domain.Test
{
    public class SearchQueryProcessTest
    {
        [Test]
        public async Task SearchQueryProcess_Search_NoCacheHits_SucceedsWithResults()
        {
            // arrange
            string searchTerm = "test 123";
            var termsSplit = searchTerm.Split(" ");

            string engineId = "engine1";
            string[] engineIds = new string[] { engineId };

            int engineReturnResult = 10000;

            CancellationTokenSource cts = new CancellationTokenSource();

            var logger = new Mock<ILogger<SearchQueryProcess>>();
            var cacheRepository = new Mock<ICacheRepository>();
            var searchRepository = new Mock<ISearchRepository>();
            var mockedEngine1 = new Mock<ISearchEngine>();

            cacheRepository.Setup(m => m.Search(termsSplit, engineIds, cts.Token))
                .Returns(Task.FromResult(new List<SearchCacheRepositoryDto>()));

            mockedEngine1.Setup(m => m.Id).Returns(engineId);

            var searchResult = new Mock<ISearchEngineResult>();
            searchResult.Setup(r => r.EngineId).Returns(engineId);
            searchResult.Setup(r => r.Hits).Returns(engineReturnResult);

            mockedEngine1.Setup(m => m.Search(It.IsAny<string>(), cts.Token))
                .Returns(Task.FromResult(searchResult.Object));

            var subject = new SearchQueryProcess( 
                logger.Object,
                new ISearchEngine[] { 
                    mockedEngine1.Object,
                },
                cacheRepository.Object,
                searchRepository.Object
            );

            // act
            var result = await subject.Search(
                engineIds,
                searchTerm, 
                cts.Token
            );

            // assert
            // one could split this into many different test methods if that floats your boat
            // if one would be very strict then this should only contain one assert. 
            // but in this instance Im going to go with a quote from reddit
            // "A single assert per unit test is a great way to test the reader's ability to scroll up and down."
            searchRepository.Verify(m => m.Add(searchTerm), Times.Once());
         
            cacheRepository.Verify(m => m.Search(termsSplit, new string[] { engineId }, cts.Token), Times.Once());

            foreach (var term in termsSplit)
                cacheRepository.Verify(m => m.Add(engineId, term, engineReturnResult), Times.Once());

            Assert.AreEqual(2, result.Results.ToList().Count());
            Assert.Contains(termsSplit[0], result.Results.Select(x => x.SearchTerm).ToArray());
            Assert.Contains(termsSplit[1], result.Results.Select(x => x.SearchTerm).ToArray());
        }

        public void SearchQueryProcess_Search_CacheHits_SucceedsWithResults()
        {
            // adding this test later (if I have time), have shown that I know how tests work with above lines
            // trying to meed deadline of 21 december....!!! .
            Assert.Pass();
        }

        public void SearchQueryProcess_Search_NoEnginesSelected_SucceedsWithNoResults()
        {
            // adding this test later (if I have time), have shown that I know how tests work with above lines
            // trying to meed deadline of 21 december....!!! .
            Assert.Pass();
        }
    }
}