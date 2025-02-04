using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using MultiLayeredCache.Domain.Models;
using MultiLayeredCache.Infrastructure.ProductWebservices;
using MultiLayeredCache.Services.ProductDataHandlers;
using Xunit;
using MassTransit;

namespace Taaghche.Books.Tests.ProductDataHandlersTest
{
    public class ProductWebServiceHandlerTest
    {
        private readonly ProductWebServiceHandler _bookWebServiceHandler;
        private readonly IProductApi _bookApi;
        private readonly IPublishEndpoint _publish;

        public ProductWebServiceHandlerTest()
        {
            //Arrange
            _bookApi = A.Fake<IProductApi>();
            _publish = A.Fake<IPublishEndpoint>();
            _bookWebServiceHandler = new ProductWebServiceHandler(_bookApi, _publish);
        }

        [Fact]
        public async Task Should_return_book_when_id_is_valid()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel bookModel = new BookModel();

            A.CallTo(()=> _bookApi.GetBookAsync(id)).Returns(bookModel);

            //Act
            var result = await _bookWebServiceHandler.GetData(id);

            //Assert
            result.Should().Be(bookModel);
        }

        [Fact]
        public async Task Should_return_null_when_id_is_not_valid()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel? bookModel = null;

            A.CallTo(() => _bookApi.GetBookAsync(id)).Returns(bookModel);

            //Act
            var result = await _bookWebServiceHandler.GetData(id);

            //Assert
            result.Should().BeNull();
        }

    }
}
