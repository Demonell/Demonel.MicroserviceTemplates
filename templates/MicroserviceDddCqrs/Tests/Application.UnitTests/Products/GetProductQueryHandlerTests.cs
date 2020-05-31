using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Queries.GetProduct;
using Application.UnitTests.Common;
using Xunit;

namespace Application.UnitTests.Products
{
    public class GetProductQueryHandlerTests : TestBase
    {
        private readonly GetProductQueryHandler _handler;
        private readonly GetProductQuery _query;

        public GetProductQueryHandlerTests()
        {
            _handler = new GetProductQueryHandler(Context, Mapper);

            _query = new GetProductQuery
            {
                Id = TestContext.TestProductCommon.Id
            };
        }

        [Fact]
        public async Task GivenValidQuery_ReturnProductVm()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.Equal(TestContext.TestProductCommon.Name, result.Name);
            Assert.Equal(TestContext.TestProductCommon.ProductType, result.ProductType);
            Assert.NotEmpty(result.Materials);
            Assert.Equal(TestContext.TestProductCommon.Materials.Count, result.Materials.Count);
            Assert.NotEmpty(result.Materials.First().Name);
        }

        [Fact]
        public async Task GivenNonExistedId_RaiseNotFoundException()
        {
            _query.Id = int.MaxValue;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _handler.Handle(_query, CancellationToken.None));
        }
    }
}