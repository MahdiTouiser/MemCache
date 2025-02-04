using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using MultiLayeredCache.Domain.Models;
using MultiLayeredCache.Infrastructure.Caching;
using MultiLayeredCache.Services.Extenstions;
using Xunit;
using MultiLayeredCache.Services.ProductDataHandlers;

namespace Taaghche.Books.Tests.ProductDataHandlersTest
{
    public class RedisCacheHandlerTest
    {
        private readonly RedisCacheHandler _redisCacheHandler;
        private readonly IProductDataHandler _nextHandler;
        private readonly ICacheProvider _cacheProvider;
        public RedisCacheHandlerTest()
        {
            var configuration = A.Fake<IConfiguration>();
            configuration["CacheConfiguration:RedisExpirationTime"] = "60";

            _cacheProvider = A.Fake<ICacheProvider>();
            _nextHandler = A.Fake<IProductDataHandler>();

            _redisCacheHandler = new RedisCacheHandler(configuration);

            _redisCacheHandler.SetNextHandler(_nextHandler);
            _redisCacheHandler.SetCacheProvider(_cacheProvider);
        }

        #region Get Data

        [Fact]
        public async Task Should_return_book_when_data_is_found_in_cache()
        {
            //Arrange
            int id = new Random().Next(1000);
            string bookAsString = "{}";
            BookModel? book = bookAsString.ToBookModel();

            A.CallTo(() => _cacheProvider.Get(id.ToString())).Returns(bookAsString);

            //Act
            var result = await _redisCacheHandler.GetData(id);

            //Assert
            result.Should().BeEquivalentTo(book);

            A.CallTo(() => _nextHandler.GetData(A<int>._)).MustNotHaveHappened();
            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Should_return_book_when_data_is_not_found_in_cache_but_id_is_valid()
        {
            //Arrange
            int id = new Random().Next(1000);
            string? bookAsString = null;
            BookModel? book = new BookModel();

            A.CallTo(() => _cacheProvider.Get(id.ToString())).Returns(bookAsString);
            A.CallTo(() => _nextHandler.GetData(id)).Returns(book);

            //Act
            var result = await _redisCacheHandler.GetData(id);

            //Assert
            result.Should().BeEquivalentTo(book);

            A.CallTo(() => _nextHandler.GetData(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Should_return_book_when_data_is_not_found_in_cache_but_id_is_not_valid()
        {
            //Arrange
            int id = new Random().Next(1000);
            string? bookAsString = null;
            BookModel? book = null;

            A.CallTo(() => _cacheProvider.Get(id.ToString())).Returns(bookAsString);
            A.CallTo(() => _nextHandler.GetData(id)).Returns(book);

            //Act
            var result = await _redisCacheHandler.GetData(id);

            //Assert
            result.Should().BeNull();

            A.CallTo(() => _nextHandler.GetData(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustNotHaveHappened();
        }

        #endregion

        #region Set Cache

        [Fact]
        public async Task Cache_must_be_set_when_book_is_not_null()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel? book = null;

            //Act
            var act = async () => await _redisCacheHandler.SetCache(id, book);

            //Assert
            await act.Should().NotThrowAsync();

            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Cache_must_not_be_set_when_book_is_null()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel? book = new BookModel();

            //Act
            var act = async () => await _redisCacheHandler.SetCache(id, book);

            //Assert
            await act.Should().NotThrowAsync();

            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustHaveHappenedOnceExactly();
        }

        #endregion

        #region Data Expired

        [Fact]
        public async Task Date_must_be_expired()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();

            //Act
            var act = async () => await _redisCacheHandler.DataExpired(id);

            //Assert
            await act.Should().NotThrowAsync();

            A.CallTo(() => _cacheProvider.Clear(id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => _nextHandler.DataExpired(id)).MustHaveHappenedOnceExactly();
        }

        #endregion
    }

}
