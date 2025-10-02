using Demo_UnitTest.Controllers;
using Demo_UnitTest.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApi.Tests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProductService> _mockService;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Test]
        public void GetAll_ReturnsOk_WithProducts()
        {
            _mockService.Setup(s => s.GetAll()).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000 }
            });

            var result = _controller.GetAll() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void GetById_ReturnsOk_WhenProductExists()
        {
            _mockService.Setup(s => s.GetById(1)).Returns(new Product { Id = 1, Name = "Phone", Price = 500 });

            var result = _controller.GetById(1) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(((Product)result!.Value!).Name, Is.EqualTo("Phone"));
        }

        [Test]
        public void GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockService.Setup(s => s.GetById(99)).Returns((Product?)null);

            var result = _controller.GetById(99);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Create_ReturnsCreatedAt_WithProduct()
        {
            var newProduct = new Product { Id = 2, Name = "Tablet", Price = 400 };
            _mockService.Setup(s => s.Create(It.IsAny<Product>())).Returns(newProduct);

            var result = _controller.Create(new Product { Name = "Tablet", Price = 400 }) as CreatedAtActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(201));
            Assert.That(((Product)result.Value!).Name, Is.EqualTo("Tablet"));
        }

        [Test]
        public void Update_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.Update(1, It.IsAny<Product>())).Returns(true);

            var result = _controller.Update(1, new Product { Name = "Updated", Price = 900 });

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void Update_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockService.Setup(s => s.Update(99, It.IsAny<Product>())).Returns(false);

            var result = _controller.Update(99, new Product { Name = "X", Price = 1 });

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.Delete(1)).Returns(true);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockService.Setup(s => s.Delete(99)).Returns(false);

            var result = _controller.Delete(99);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
