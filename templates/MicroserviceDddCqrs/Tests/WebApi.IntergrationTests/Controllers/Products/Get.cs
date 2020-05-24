using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Products.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.IntergrationTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace WebApi.IntergrationTests.Controllers.Products
{
    public class Get : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public Get(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            factory.Output = output;
            _client = _factory.CreateClientWithTestAuth();
        }

        [Fact]
        public async Task GivenValidRequest_ReturnsProductVm()
        {
            var response = await _client.GetAsync($"/api/products/get/{_factory.TestContext.TestProduct1.Id}");

            response.EnsureSuccessStatusCode();

            var entity = await Utilities.GetResponseContent<ProductVm>(response);

            Assert.NotNull(entity);
            Assert.IsType<ProductVm>(entity);
            Assert.NotEmpty(entity.Name);
            Assert.NotEmpty(entity.Materials);
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsValidationProblemDetails()
        {
            var response = await _client.GetAsync($"/api/products/get/notnumberatall");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var entity = await Utilities.GetResponseContent<ValidationProblemDetails>(response);

            Assert.Equal("id", entity.Errors.First().Key);
        }

        [Fact]
        public async Task GivenNonExistedId_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync($"/api/products/get/{int.MaxValue}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}