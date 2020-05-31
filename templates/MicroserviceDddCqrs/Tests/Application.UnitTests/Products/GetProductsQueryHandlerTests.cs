using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Queries.GetProducts;
using Application.UnitTests.Common;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Application.UnitTests.Products
{
    public class GetProductsQueryHandlerTests : TestBase
    {
        private readonly ITestOutputHelper _output;
        private readonly GetProductsQueryHandler _handler;
        private readonly GetProductsQuery _query;

        public GetProductsQueryHandlerTests(ITestOutputHelper output)
        {
            _output = output;

            _handler = new GetProductsQueryHandler(Context, Mapper);

            _query = new GetProductsQuery();
        }

        [Fact]
        public async Task GivenValidQuery_ReturnTotalListOfProductVm()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            var productsCount = TestContext.Context.Products.Count();
            Assert.Equal(productsCount, result.Total);
            Assert.Equal(productsCount, result.Items.Count);
            Assert.Equal(TestContext.TestProductCommon.Name, result.Items.First().Name);
        }

        [Fact]
        public async Task GivenSkip1Take1_ReturnTestProductVip1()
        {
            _query.Skip = 1;
            _query.Take = 1;

            var result = await _handler.Handle(_query, CancellationToken.None);

            var productsCount = TestContext.Context.Products.Count();
            Assert.Equal(productsCount, result.Total);
            Assert.Single(result.Items);
            Assert.Equal(TestContext.TestProductVip1.Name, result.Items.First().Name);
        }

        [Fact]
        public async Task GivenProductNameFilter_ReturnTestProductCommon1()
        {
            _query.Name = "Common";

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.Single(result.Items);
            Assert.Equal(TestContext.TestProductCommon.Id, result.Items.First().Id);
        }

        [Fact]
        public async Task GivenMaterialNameFilter_Return2VipProducts()
        {
            _query.MaterialName = "steel";

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.Equal(2, result.Total);
            Assert.Equal(TestContext.TestProductVip1.Id, result.Items.First().Id);
            Assert.Equal(TestContext.TestProductVip2.Id, result.Items.Last().Id);
        }

        [Fact]
        public async Task GivenMaterialNameSort_ReturnSortedTotalListOfProductVm()
        {
            _query.Sort = "+materialName";

            var result = await _handler.Handle(_query, CancellationToken.None);

            var materials = result.Items.Select(i => i.Materials.OrderBy(m => m.Name).Select(m => m.Name).FirstOrDefault());
            _output.WriteLine(JsonConvert.SerializeObject(materials, Formatting.Indented));

            Assert.Equal(TestContext.TestProductCommon.Id, result.Items[0].Id);
            Assert.Equal(TestContext.TestProductVip1.Id, result.Items[1].Id);
            Assert.Equal(TestContext.TestProductVip2.Id, result.Items[2].Id);


            _query.Sort = "-materialName";

            result = await _handler.Handle(_query, CancellationToken.None);

            materials = result.Items.Select(i => i.Materials.OrderByDescending(m => m.Name).Select(m => m.Name).FirstOrDefault());
            _output.WriteLine(JsonConvert.SerializeObject(materials, Formatting.Indented));

            Assert.Equal(TestContext.TestProductVip2.Id, result.Items[0].Id);
            Assert.Equal(TestContext.TestProductVip1.Id, result.Items[1].Id);
            Assert.Equal(TestContext.TestProductCommon.Id, result.Items[2].Id);
        }

        [Fact]
        public async Task GivenMultipleSorts_ReturnMultiSortedTotalListOfProductVm()
        {
            _query.Sort = "+productType,-id";

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.Equal(TestContext.TestProductCommon.Id, result.Items[0].Id);
            Assert.Equal(TestContext.TestProductVip2.Id, result.Items[1].Id);
            Assert.Equal(TestContext.TestProductVip1.Id, result.Items[2].Id);


            _query.Sort = "-productType,-id";

            result = await _handler.Handle(_query, CancellationToken.None);

            Assert.Equal(TestContext.TestProductVip2.Id, result.Items[0].Id);
            Assert.Equal(TestContext.TestProductVip1.Id, result.Items[1].Id);
            Assert.Equal(TestContext.TestProductCommon.Id, result.Items[2].Id);
        }
    }
}