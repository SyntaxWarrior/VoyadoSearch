using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Contracts;
using VoyadoSearch.Domain.Abstract.DTO;
using VoyadoSearch.Domain.Abstract.Processes;
using VoyagoSearch.Web.Controllers;

namespace VoyagiSearch.Test
{
    public class EngineControllerTest
    {
        [Test]
        public void EngineController_GetSearchEngines_ReturnsListSuccessfully()
        {
            // arrange
            CancellationTokenSource cts = new CancellationTokenSource();
            var searchEngine = new Mock<ISearchEngineListProcess>();

            var searchResultList = new List<SearchEngineDomainDto>();
            searchResultList.Add(new SearchEngineDomainDto("engine1", "Engine 1"));
            searchResultList.Add(new SearchEngineDomainDto("engine2", "Engine 2"));

            searchEngine.Setup(e => e.List()).Returns(searchResultList.ToArray());

            var subject = new EngineController(searchEngine.Object);

            // act
            var resultAction = subject.GetSearchEngines(cts.Token);
            var result = GetObjectResultContent(resultAction).ToList();

            // assert
            Assert.AreEqual(2, result.Count);
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == "engine1" && r.DisplayName == "Engine 1"));
            Assert.IsNotNull(result.FirstOrDefault(r => r.Id == "engine2" && r.DisplayName == "Engine 2"));
        }

        private T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}