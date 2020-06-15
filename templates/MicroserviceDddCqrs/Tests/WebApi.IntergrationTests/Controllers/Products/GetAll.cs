using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.IntergrationTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace WebApi.IntergrationTests.Controllers.Products
{
    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public GetAll(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            factory.Output = output;
            _client = _factory.CreateClientWithTestAuth();
        }

        [Fact]
        public async Task GivenValidRequest_ReturnsProductVm()
        {
            var response = await _client.GetAsync($"/api/products?productType=1&sort=-id{Uri.EscapeDataString(",+name")}");

            response.EnsureSuccessStatusCode();

            var entity = await Utilities.GetResponseContent<TotalList<ProductVm>>(response);
            _factory.Output.WriteLine(JsonConvert.SerializeObject(entity));

            Assert.NotNull(entity);
            Assert.IsType<TotalList<ProductVm>>(entity);
            Assert.Single(entity.Items);
            Assert.NotEmpty(entity.Items.First().Name);
            Assert.NotEmpty(entity.Items.First().Materials.First().Name);
        }

        [Fact]
        public async Task GivenDeliveryDateRange_ReturnsProductVm()
        {
            var tomorrow = DateTimeOffset.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            _factory.Output.WriteLine(tomorrow);

            var response = await _client.GetAsync($"/api/products?deliveryDate.from={tomorrow}");

            response.EnsureSuccessStatusCode();

            var entity = await Utilities.GetResponseContent<TotalList<ProductVm>>(response);
            _factory.Output.WriteLine(JsonConvert.SerializeObject(entity));

            Assert.NotNull(entity);
            Assert.IsType<TotalList<ProductVm>>(entity);
            Assert.Single(entity.Items);
            Assert.NotEmpty(entity.Items.First().Name);
            Assert.NotEmpty(entity.Items.First().Materials.First().Name);
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsValidationProblemDetails()
        {
            var response = await _client.GetAsync("/api/products?sort=-nonExistedField");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var entity = await Utilities.GetResponseContent<ValidationProblemDetails>(response);

            Assert.Equal("Sort", entity.Errors.First().Key);
        }
    }
}