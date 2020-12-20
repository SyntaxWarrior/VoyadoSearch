using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using VoyadoSearch.Persistence.Entities;
using VoyadoSearch.Persistence.Repositories;

namespace VoyagoSearch.Domain.Test
{
    public class CacheRepositoryTest
    {
        [Test]
        public void CacheRepository_Add_Succeeds()
        {
            // arrange
            CancellationTokenSource cts = new CancellationTokenSource();
            var logger = new Mock<ILogger<CacheRepository>>();

            var targetList = new List<SearchCache>();

            var dbSet = new Mock<DbSet<SearchCache>>();
            dbSet.Setup(d => d.Add(It.IsAny<SearchCache>()))
                .Callback<SearchCache>((s) => targetList.Add(s));

            var context = new Mock<IApplicationDbContext>();
            context.Setup(c => c.SearchCache).Returns(dbSet.Object);

            Mock<IConfiguration> configuration = SetupIConfiguration();

            var subject = new CacheRepository(context.Object, configuration.Object);

            // act
            string engine = "engine1";
            string term = "hello";
            long hits = 10000;

            subject.Add(engine, term, hits);

            // assert
            var added = targetList.FirstOrDefault();
            Assert.IsNotNull(added);
            Assert.AreEqual(engine, added.EngineId);
            Assert.AreEqual(term, added.Term);
            Assert.AreEqual(hits, added.Hits);
        }

        [Test]
        public async Task CacheRepository_Search_SucceedsWithResults()
        {
            // arrange
            CancellationTokenSource cts = new CancellationTokenSource();
            var logger = new Mock<ILogger<CacheRepository>>();

            var mockDbSet = CreateQueryableSearchCacheDbSet();

            var context = new Mock<IApplicationDbContext>();
            context.Setup(c => c.SearchCache).Returns(mockDbSet.Object);

            Mock<IConfiguration> configuration = SetupIConfiguration();

            var subject = new CacheRepository(context.Object, configuration.Object);

            // act
            string[] terms = new string[] { "hello", "world" };
            string[] engines = new string[] { "engine1" };

            var result = await subject.Search(terms, engines, cts.Token);

            // assert(s)
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var theOneResult = result.First();
            Assert.AreEqual(theOneResult.EngineId, engines.FirstOrDefault());
            Assert.Contains(theOneResult.Term, terms);
            Assert.AreEqual(theOneResult.Hits, 1);
        }

        private static Mock<IConfiguration> SetupIConfiguration()
        {
            var configuration = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("2");
            configuration.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
            configuration.Setup(a => a.GetSection("Cache:CacheLifeTimeDays")).Returns(configurationSection.Object);
            return configuration;
        }

        private static Mock<DbSet<SearchCache>> CreateQueryableSearchCacheDbSet()
        {
            var sourceList = new List<SearchCache>();
            sourceList.Add(new SearchCache()
            {
                Created = DateTimeOffset.UtcNow,
                EngineId = "engine1",
                Hits = 1,
                Term = "hello"
            });
            sourceList.Add(new SearchCache()
            {
                Created = DateTimeOffset.UtcNow.AddDays(-100),
                EngineId = "engine1",
                Hits = 2,
                Term = "world"
            });
            sourceList.Add(new SearchCache()
            {
                Created = DateTimeOffset.UtcNow,
                EngineId = "engine1",
                Hits = 3,
                Term = "space"
            });
            sourceList.Add(new SearchCache()
            {
                Created = DateTimeOffset.UtcNow,
                EngineId = "engine2",
                Hits = 4,
                Term = "odyssey"
            });

            return sourceList.AsQueryable().BuildMockDbSet();
        }
    }
}