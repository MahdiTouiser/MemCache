using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;
using MultiLayeredCache.Infrastructure.Caching;
using Xunit;

namespace Taaghche.Books.Tests.CacheTests
{
    public class RedisCacheTest
    {
        private readonly RedisCacheProvider _redisCacheProvider;
        private readonly IDistributedCache _fakeRedisCache;
        public RedisCacheTest()
        {
            //Arrange
            _fakeRedisCache = A.Fake<IDistributedCache>();
            _redisCacheProvider = new RedisCacheProvider(_fakeRedisCache);
        }

        [Fact]
        public async Task Should_return_book_when_key_is_valid()
        {
            //Arrange
            var fakeKey = new Random().Next(0, 1000).ToString();

            //Act
            var act = async () => await _redisCacheProvider.Get(fakeKey);

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_return_null_when_key_is_valid()
        {
            var fakeKey = new Random().Next(0, 1000).ToString();

            //Act
            var result = await _redisCacheProvider.Get(fakeKey);

            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Should_clear_cache()
        {
            //Arrange
            var fakeKey = new Random().Next(1000).ToString();

            A.CallTo(() => _fakeRedisCache.RemoveAsync(fakeKey, A<CancellationToken>._));

            var act = async () => await _redisCacheProvider.Clear(fakeKey);

            //Assert
            await act.Should().NotThrowAsync();
            A.CallTo(() => _fakeRedisCache.RemoveAsync(fakeKey, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Should_set_cache()
        {
            //Arrange
            var fakeExpirationDate = new Random().Next(1000);
            var fakeKey = new Random().Next(1000).ToString();
            var fakeBookValue = "fake book value";

            var act = async () => await _redisCacheProvider.Set(fakeKey, fakeBookValue, fakeExpirationDate);

            //Assert
            await act.Should().NotThrowAsync();
        }
    }
}
