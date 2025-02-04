using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiLayeredCache.API.Controllers;
using MultiLayeredCache.Domain.Models;
using MultiLayeredCache.Services.Services;
using Xunit;

namespace Taaghche.Books.Tests.ControllersTest
{
    public class BookControllerTest
    {
        private readonly ProductController _bookController;
        private readonly IProductService _bookService;
        public BookControllerTest()
        {
            _bookService = A.Fake<IProductService>();
            _bookController = new ProductController(_bookService);
        }

        [Fact]
        public async Task Should_return_ok_when_book_exists()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel book = new BookModel();

            A.CallTo(() => _bookService.GetBookByIdAsync(id)).Returns(book);

            //Act
            var response = await _bookController.GetBook(id);
            var result = (OkObjectResult)response;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Should_return_not_found_when_book_deos_not_exist()
        {
            //Arrange
            int id = new Random().Next(1000);
            BookModel book = null;

            A.CallTo(() => _bookService.GetBookByIdAsync(id)).Returns(book);

            //Act
            var response = await _bookController.GetBook(id);
            var result = (NotFoundResult)response;

            //Assert
            response.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }
    }
}
