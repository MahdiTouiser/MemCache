using FakeItEasy;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using MultiLayeredCache.Domain.Models;
using MultiLayeredCache.Infrastructure.Caching;
using MultiLayeredCache.Services.ProductDataHandlers;
using MultiLayeredCache.Services.Extenstions;
using Xunit;

namespace Taaghche.Books.Tests.BookDataHandlersTest
{
    public class InMemmoryCacheHandlerTest
    {
        private readonly InMemoryCacheHandler _inMemmoryCacheHandler;
        private readonly IProductDataHandler _nextHandler;
        private readonly ICacheProvider _cacheProvider;
        public InMemmoryCacheHandlerTest()
        {
            var configuration = A.Fake<IConfiguration>();
            configuration["CacheConfiguration:InMemoryCacheExpirationTime"] = "60";
            configuration["CacheConfiguration:MissDataExpirationTime"] = "60";

            _cacheProvider = A.Fake<ICacheProvider>();
            _nextHandler = A.Fake<IProductDataHandler>();

            _inMemmoryCacheHandler = new InMemoryCacheHandler(configuration,A.Fake<IPublishEndpoint>());

            _inMemmoryCacheHandler.SetNextHandler(_nextHandler);
            _inMemmoryCacheHandler.SetCacheProvider(_cacheProvider);
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
            var result = await _inMemmoryCacheHandler.GetData(id);

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
            var result = await _inMemmoryCacheHandler.GetData(id);

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
            var result = await _inMemmoryCacheHandler.GetData(id);

            //Assert
            result.Should().BeNull();

            A.CallTo(() => _nextHandler.GetData(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustNotHaveHappened();
        }

        #endregion

        #region Set Cache

        [Fact]
        public async Task Cache_must_be_set()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel? book = null;

            //Act
            var act = async () => await _inMemmoryCacheHandler.SetCache(id, book);

            //Assert
            await act.Should().NotThrowAsync();

            A.CallTo(() => _cacheProvider.Set(id.ToString(), book.AsString(), A<int>._)).MustNotHaveHappened();
        }


        #endregion

        #region Data Expired

        [Fact]
        public async Task Date_must_be_expired()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var act = async () => await _inMemmoryCacheHandler.DataExpired(id);

            //Assert
            await act.Should().NotThrowAsync();

            A.CallTo(() => _cacheProvider.Clear(id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => _nextHandler.DataExpired(id)).MustHaveHappenedOnceExactly();
        }

        #endregion
    }

}
