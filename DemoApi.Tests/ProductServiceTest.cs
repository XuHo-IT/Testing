using Demo_UnitTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApi.Tests
{
    [TestFixture]
    public class ProductServiceTest
    {
        private IProductService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ProductService();
        }

        [Test]
        public void Create_ShouldAddNewProduct()
        {
            var product = new Product { Name = "Laptop", Price = 1200m };

            var result = _service.Create(product);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(_service.GetAll().Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetById_ShouldReturnCorrectProduct()
        {
            var created = _service.Create(new Product { Name = "Phone", Price = 500m });

            var result = _service.GetById(created.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Phone"));
        }

        [Test]
        public void Update_ShouldModifyProduct_WhenExists()
        {
            var created = _service.Create(new Product { Name = "Tablet", Price = 300m });

            var updated = _service.Update(created.Id, new Product { Name = "Tablet Pro", Price = 400m });

            Assert.That(updated, Is.True);
            var result = _service.GetById(created.Id);
            Assert.That(result!.Name, Is.EqualTo("Tablet Pro"));
            Assert.That(result.Price, Is.EqualTo(400m));
        }

        [Test]
        public void Delete_ShouldRemoveProduct_WhenExists()
        {
            var created = _service.Create(new Product { Name = "Watch", Price = 200m });

            var deleted = _service.Delete(created.Id);

            Assert.That(deleted, Is.True);
            Assert.That(_service.GetAll().Any(), Is.False);
        }

        [Test]
        public void Delete_ShouldReturnFalse_WhenNotExists()
        {
            var deleted = _service.Delete(999);

            Assert.That(deleted, Is.False);
        }
    }
}
