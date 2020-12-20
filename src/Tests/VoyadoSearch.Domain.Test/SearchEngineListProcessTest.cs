using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using VoyadoSearch.Domain.Processes;
using VoyadoSearch.Integraions.Search.Abstract;

namespace VoyagoSearch.Domain.Test
{
    public class Tests
    {
        [Test]
        public void SearchEngineListProcess_Id_PropertyReturnsEngineId_Succeeds()
        {
            // arrange
            var mockedEngine1 = new Mock<ISearchEngine>();
            var mockedEngine2 = new Mock<ISearchEngine>();

            mockedEngine1.Setup(m => m.Id).Returns("engine1");
            mockedEngine2.Setup(m => m.Id).Returns("engine2");

            var subject = new SearchEngineListProcess(new ISearchEngine[] { 
                mockedEngine1.Object,
                mockedEngine2.Object
            });

            // act
            var result = subject.List();

            // assert
            Assert.IsNotNull(Array.Find(result, x => x.Id == "engine1"));
            Assert.IsNotNull(Array.Find(result, x => x.Id == "engine2"));
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void SearchEngineListProcess_DisplayName_PropertyReturnsEngineId_Succeeds()
        {
            // arrange
            var mockedEngine1 = new Mock<ISearchEngine>();
            var mockedEngine2 = new Mock<ISearchEngine>();

            mockedEngine1.Setup(m => m.DisplayName).Returns("Display1");
            mockedEngine2.Setup(m => m.DisplayName).Returns("Display2");

            var subject = new SearchEngineListProcess(new ISearchEngine[] {
                mockedEngine1.Object,
                mockedEngine2.Object
            });

            // act
            var result = subject.List();

            // assert
            Assert.IsNotNull(Array.Find(result, x => x.DisplayName == "Display1"));
            Assert.IsNotNull(Array.Find(result, x => x.DisplayName == "Display2"));
            Assert.AreEqual(2, result.Length);
        }
    }
}