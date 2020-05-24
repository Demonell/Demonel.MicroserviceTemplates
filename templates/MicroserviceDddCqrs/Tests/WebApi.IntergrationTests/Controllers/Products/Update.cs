using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Commands.UpdateProduct;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.IntergrationTests.Common;
using WebApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace WebApi.IntergrationTests.Controllers.Products
{
    public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private readonly UpdateProductCommand _command;

        public Update(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            factory.Output = output;
            _client = _factory.CreateClientWithTestAuth();

            _command = new UpdateProductCommand
            {
                Id = _factory.TestContext.TestProduct1.Id,
                Name = "test",
                ProductType = ProductType.Vip,
                Materials = null
            };
        }

        [Fact]
        public async Task GivenValidRequest_ReturnSuccessStatusCode()
        {
            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PutAsync($"/api/products/update/{_command.Id}", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsValidationProblemDetails()
        {
            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PutAsync("/api/products/update/InvalidId", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var model = await Utilities.GetResponseContent<ValidationProblemDetails>(response);

            Assert.Equal("id", model.Errors.First().Key);
        }

        [Fact]
        public async Task GivenNonExistedId_ReturnsNotFoundStatusCode()
        {
            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PutAsync($"/api/products/update/{int.MaxValue}", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GivenAlreadyUsedName_ReturnsProductNameAlreadyInUseException()
        {
            _command.Name = _factory.TestContext.TestProduct2.Name;

            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PutAsync($"/api/products/update/{_command.Id}", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var model = await Utilities.GetResponseContent<ApiErrorVm>(response);

            Assert.Equal(nameof(ProductNameAlreadyInUseException), model.Code);
        }
    }
}