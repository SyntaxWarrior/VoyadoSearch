using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Domain.Abstract.DTO;
using VoyadoSearch.Domain.Abstract.Processes;
using VoyadoSearch.Web.Controllers;

namespace VoyagiSearch.Test
{
    public class SearchControllerTest
    {
        [Test]
        public async Task SearchController_Query_ReturnsResultSuccessfully()
        {
            // arrange
            CancellationTokenSource cts = new CancellationTokenSource();
            var searchQuery = new Mock<ISearchQueryProcess>();

            var searchResultList = new List<SearchEngineResultDomainDto>();
            searchResultList.Add(new SearchEngineResultDomainDto("engine1", "Engine 1", 1, "foo"));
            searchResultList.Add(new SearchEngineResultDomainDto("engine2", "Engine 2", 2, "bar"));

            searchQuery.Setup(e => e.Search(It.IsAny<string[]>(), It.IsAny<string>(), cts.Token))
                .Returns(Task.FromResult(new SearchResultDomainDto(searchResultList)));

            var subject = new SearchController(searchQuery.Object, null);

            // act
            var resultAction = await subject.Query(
                new string[] { "foo", "bar" },
                "foo bar",
                cts.Token
            );

            var result = GetObjectResultContent(resultAction);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Results.Count());
            Assert.IsNotNull(result.Results.FirstOrDefault(r => r.EngineId == "engine1" && r.SearchTerm == "foo" && r.HitCount == 1));
            Assert.IsNotNull(result.Results.FirstOrDefault(r => r.EngineId == "engine2" && r.SearchTerm == "bar" && r.HitCount == 2));
        }

        private T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}