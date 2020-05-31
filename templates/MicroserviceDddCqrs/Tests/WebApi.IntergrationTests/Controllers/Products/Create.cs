using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Commands.CreateProduct;
using Application.Products.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.IntergrationTests.Common;
using WebApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace WebApi.IntergrationTests.Controllers.Products
{
    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private readonly CreateProductCommand _command;

        public Create(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            factory.Output = output;
            _client = _factory.CreateClientWithTestAuth();

            _command = new CreateProductCommand
            {
                Name = "test",
                ProductType = ProductType.Vip,
                Materials = new List<MaterialVm>
                {
                    new MaterialVm { Name = "wood", Durability = TimeSpan.FromDays(365 * 4)},
                    new MaterialVm { Name = "iron", Durability = TimeSpan.FromDays(365 * 8)}
                }
            };
        }

        [Fact]
        public async Task GivenValidRequest_ReturnsCreatedProductId()
        {
            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PostAsync("/api/products/create", content);

            response.EnsureSuccessStatusCode();

            var id = await Utilities.GetResponseContent<int>(response);

            Assert.IsType<int>(id);
            Assert.True(id > 0);
        }

        [Fact]
        public async Task GivenEmptyName_ReturnsValidationProblemDetails()
        {
            _command.Name = "";

            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PostAsync("/api/products/create", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var model = await Utilities.GetResponseContent<ValidationProblemDetails>(response);

            Assert.Equal(nameof(_command.Name), model.Errors.First().Key);
        }

        [Fact]
        public async Task GivenAlreadyUsedName_ReturnsProductNameAlreadyInUseException()
        {
            _command.Name = _factory.TestContext.TestProductCommon.Name;

            var content = Utilities.GetRequestContent(_command);

            var response = await _client.PostAsync("/api/products/create", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var model = await Utilities.GetResponseContent<ApiErrorVm>(response);

            Assert.Equal(nameof(ProductNameAlreadyInUseException), model.Code);
        }
    }
}