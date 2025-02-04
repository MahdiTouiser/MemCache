using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using MultiLayeredCache.Infrastructure.Caching;
using Xunit;

namespace Taaghche.Books.Tests.CacheTests
{
    public class InMemmoryCacheTest
    {
        private readonly InMemoryCacheProvider _inMemoryCacheProvider;
        private readonly IMemoryCache _fakeMemoryCache;
        public InMemmoryCacheTest()
        {
            //Arrange
            _fakeMemoryCache = A.Fake<IMemoryCache>();
            _inMemoryCacheProvider = new InMemoryCacheProvider(_fakeMemoryCache);
        }

        [Fact]
        public async Task Should_return_book_when_key_is_valid()
        {
            //Arrange
            var fakeKey = new Random().Next(0,1000).ToString();

            //Act
            var act = async () => await _inMemoryCacheProvider.Get(fakeKey);

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_return_null_when_key_is_valid()
        {
            //Arrange
            var fakeKey = new Random().Next(0, 1000).ToString();

            //Act
            var result = await _inMemoryCacheProvider.Get(fakeKey);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_clear_cache()
        {
            //Arrange
            var fakeKey = new Random().Next(1000).ToString();

            A.CallTo(() => _fakeMemoryCache.Remove(fakeKey));

            var act = async () => await _inMemoryCacheProvider.Clear(fakeKey);

            //Assert
            await act.Should().NotThrowAsync();
            A.CallTo(()=> _fakeMemoryCache.Remove(fakeKey)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Should_set_cache()
        {
            //Arrange
            var fakeExpirationDate = new Random().Next(1000);
            var fakeKey = new Random().Next(1000).ToString();
            var fakeBookValue = "fake book value";

            var act = async () => await _inMemoryCacheProvider.Set(fakeKey, fakeBookValue, fakeExpirationDate);

            //Assert
            await act.Should().NotThrowAsync();
        }
    }
}
