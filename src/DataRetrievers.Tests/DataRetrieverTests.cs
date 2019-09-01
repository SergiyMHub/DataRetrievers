using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataRetrievers.Tests
{
    public class DataRetrieverTests
    {
        private FakeProjection[] _fakeProjections;
        private InMemoryDataRetriever<FakeProjection> _dataRetriever;

        [SetUp]
        public void Setup()
        {
            _fakeProjections = new[] 
            {
                new FakeProjection { Id = 2, Name = "Abra"},
                new FakeProjection { Id = 1, Name = "Abra-Cadabra"},
                new FakeProjection { Id = 3, Name = "Oz"},
                new FakeProjection { Id = 5, Name = "Buzz"},
            };

            _dataRetriever = new InMemoryDataRetriever<FakeProjection>(_fakeProjections);
        }


        [Test]
        public async Task RetrieveAsync_ShouldReturnEmptyDataPage()
        {
            //arrange
            var dataRetriever = new InMemoryDataRetriever<FakeProjection>(Enumerable.Empty<FakeProjection>());
            var predicates = Enumerable.Empty<Expression<Func<FakeProjection, bool>>>();

            //act
            var result = await dataRetriever.RetrieveAsync(predicates, new []{ Sorting<FakeProjection>.Ascending(c => c.Id)});

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalRecords);
            Assert.AreEqual(0, result.Data.Count());
        }

        [Test]
        public async Task RetrieveAsync_ShouldRunPredicateValidation()
        {
            //arrange
            var predicates = new Expression<Func<FakeProjection, bool>>[] {
                ((p)=> p.Id + p.Id == 3)
            };
            //act
            AsyncTestDelegate action = async () => await _dataRetriever.RetrieveAsync(predicates, new[] { Sorting<FakeProjection>.Ascending(c => c.Id) });

            //assert
            Assert.AreEqual("predicates[0]",
                Assert.ThrowsAsync<ArgumentException>(action).ParamName);
        }


        [Test]
        public async Task RetrieveAsync_ShouldReturnRecordsConformingPredicates()
        {
            //arrange
            var predicates = new Expression<Func<FakeProjection, bool>>[] {
                ((p)=> p.Id == 3)
            };

            //act
            var result = await _dataRetriever.RetrieveAsync(predicates, new[] { Sorting<FakeProjection>.Ascending(c => c.Id) });

            //assert
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result.Data);
            Assert.AreEqual(1, result.Data.Count());
            Assert.AreEqual(_fakeProjections[2].Id, result.Data.First().Id);
            Assert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public async Task RetrieveAsync_ShouldReturnSortedRecords()
        {
            //arrange
            var predicates = Enumerable.Empty<Expression<Func<FakeProjection, bool>>>();
            var sorting = new[] {
                Sorting<FakeProjection>.Ascending(c => c.Id),
                Sorting<FakeProjection>.Ascending(c => c.Name),
            };

            //act
            var result = await _dataRetriever.RetrieveAsync(predicates, sorting, 0, (uint)_fakeProjections.Length);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_fakeProjections.Length, result.TotalRecords);
            Assert.AreEqual(_fakeProjections.Length, result.Data.Count());
            CollectionAssert.AreEqual(_fakeProjections.OrderBy(r => r.Id).ThenBy(r => r.Name).ToArray(), result.Data);

        }

        [Test]
        public async Task RetrieveAsync_ShouldApplyFramingOperations()
        {
            //arrange
            var predicates = Enumerable.Empty<Expression<Func<FakeProjection, bool>>>();
            var sorting = new[] { Sorting<FakeProjection>.Ascending(c => c.Id) };
            var take = 3U;
            var skip = 1U;

            //act
            var result = await _dataRetriever.RetrieveAsync(predicates, sorting, skip, take);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_fakeProjections.Length, result.TotalRecords);
            Assert.AreEqual(take, result.Data.Count());
            CollectionAssert.AreEqual(
                _fakeProjections
                    .OrderBy(r => r.Id)
                    .Skip((int)skip)
                    .Take((int)take)
                    .ToArray(), 
                result.Data);
        }
    }
}
