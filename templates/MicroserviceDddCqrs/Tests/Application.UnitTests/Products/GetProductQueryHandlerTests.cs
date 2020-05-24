using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Models;
using Application.Products.Queries.GetProduct;
using Application.UnitTests.Common;
using Xunit;

namespace Application.UnitTests.Products
{
    public class GetProductQueryHandlerTests : TestBase
    {
        private readonly GetProductQuery _query;

        public GetProductQueryHandlerTests()
        {
            _query = new GetProductQuery
            {
                Id = TestContext.TestProduct1.Id
            };
        }

        [Fact]
        public async Task GivenValidQuery_ReturnProductVm()
        {
            var handler = new GetProductQueryHandler(Context, Mapper);

            var result = await handler.Handle(_query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<ProductVm>(result);
            Assert.Equal(TestContext.TestProduct1.Name, result.Name);
            Assert.Equal(TestContext.TestProduct1.ProductType, result.ProductType);
            Assert.NotEmpty(result.Materials);
            Assert.Equal(TestContext.TestProduct1.Materials.Count, result.Materials.Count);
            Assert.NotEmpty(result.Materials.First().Name);
        }

        [Fact]
        public async Task GivenNonExistedId_RaiseNotFoundException()
        {
            _query.Id = int.MaxValue;

            var handler = new GetProductQueryHandler(Context, Mapper);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(_query, CancellationToken.None));
        }
    }
}