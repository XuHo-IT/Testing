using Demo_UnitTest.Entity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DemoApi.IntegrationTests
{
    [TestFixture]
    public class ProductsApiTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task PostProduct_ShouldReturnCreated()
        {
            var product = new Product { Name = "Phone", Price = 500 };

            var response = await _client.PostAsJsonAsync("/api/products", product);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/products");

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task CrudFlow_ShouldWork()
        {
            // Create
            var newProduct = new Product { Name = "Laptop", Price = 1000 };
            var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
            var created = await createResponse.Content.ReadFromJsonAsync<Product>();

            Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
            Assert.That(created, Is.Not.Null);

            // Get
            var getResponse = await _client.GetAsync($"/api/products/{created!.Id}");
            Assert.That(getResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            // Update
            var updated = new Product { Id = created.Id, Name = "Laptop Pro", Price = 1200 };
            var updateResponse = await _client.PutAsJsonAsync($"/api/products/{created.Id}", updated);
            Assert.That(updateResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/products/{created.Id}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));
        }
    }
}
