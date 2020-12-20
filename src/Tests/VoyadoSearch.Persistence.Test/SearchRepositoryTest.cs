using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoyadoSearch.Persistence;
using VoyadoSearch.Persistence.Abstract.Entities;
using VoyadoSearch.Persistence.Entities;
using VoyadoSearch.Persistence.Repositories;

namespace VoyagoSearch.Domain.Test
{
    public class SearchRepositoryTest
    {
        [Test]
        public void SearchRepositoryTest_Add_Succeeds()
        {
            // arrange
            var logger = new Mock<ILogger<SearchRepository>>();

            var targetList = new List<SearchHistory>();

            var dbSet = new Mock<DbSet<SearchHistory>>();
            dbSet.Setup(d => d.Add(It.IsAny<SearchHistory>()))
                .Callback<SearchHistory>((s) => targetList.Add(s));

            var context = new Mock<IApplicationDbContext>();
            context.Setup(c => c.SearchHistory).Returns(dbSet.Object);

            var subject = new SearchRepository(context.Object);

            // act
            string term = "hello";
            subject.Add(term);

            // assert
            var added = targetList.FirstOrDefault();
            Assert.IsNotNull(added);
            Assert.AreEqual(term, added.Term);
        }

        [Test]
        public async Task SearchRepositoryTest_GetPrevious_SucceedsWithResults()
        {
            // arrange
            CancellationTokenSource cts = new CancellationTokenSource();
            var logger = new Mock<ILogger<CacheRepository>>();

            var mockDbSet = CreateQueryableSearchHistoryDbSet();

            var context = new Mock<IApplicationDbContext>();
            context.Setup(c => c.SearchHistory).Returns(mockDbSet.Object);

            var subject = new SearchRepository(context.Object);

            // act
            var result = await subject.GetHistoric(2, cts.Token);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(a => a.Term == "hello"));
            Assert.IsTrue(result.Any(a => a.Term == "world"));
        }

        private static Mock<DbSet<SearchHistory>> CreateQueryableSearchHistoryDbSet()
        {
            var sourceList = new List<SearchHistory>();
            sourceList.Add(new SearchHistory()
            {
                Created = DateTimeOffset.UtcNow,
                Term = "hello"
            });
            sourceList.Add(new SearchHistory()
            {
                Created = DateTimeOffset.UtcNow.AddDays(-1),
                Term = "world"
            });
            sourceList.Add(new SearchHistory()
            {
                Created = DateTimeOffset.UtcNow.AddDays(-2),
                Term = "no"
            });

            return sourceList.AsQueryable().BuildMockDbSet();
        }
    }
}