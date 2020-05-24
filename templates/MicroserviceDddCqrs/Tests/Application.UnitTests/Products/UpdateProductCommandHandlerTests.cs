using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Models;
using Application.UnitTests.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.UnitTests.Products
{
    public class UpdateProductCommandHandlerTests : TestBase
    {
        private readonly UpdateProductCommand _command;

        public UpdateProductCommandHandlerTests()
        {
            _command = new UpdateProductCommand
            {
                Id = TestContext.TestProduct1.Id,
                Name = "new name",
                ProductType = ProductType.Vip,
                Materials = new List<MaterialVm>
                {
                    new MaterialVm{ Name= "wood", Durability = TimeSpan.FromDays(2 * 365)},
                    new MaterialVm{ Name= "iron", Durability = TimeSpan.FromDays(4 * 365)}
                }
            };
        }

        [Fact]
        public async Task GivenValidCommand_Pass()
        {
            var handler = new UpdateProductCommandHandler(Context, Mapper);

            await handler.Handle(_command, CancellationToken.None);

            var product = await Context.Products.FirstOrDefaultAsync(u => u.Id == _command.Id);

            Assert.NotNull(product);
            Assert.Equal(_command.Name, product.Name);
            Assert.Equal(TestUserId, product.LastModifiedBy);
            Assert.Equal(DateTime.Now.Year, product.LastModified?.Year);
        }

        [Fact]
        public async Task GivenSameName_Pass()
        {
            _command.Name = TestContext.TestProduct1.Name;

            var handler = new UpdateProductCommandHandler(Context, Mapper);

            await handler.Handle(_command, CancellationToken.None);
        }

        [Fact]
        public async Task GivenNonExistedId_RaiseNotFoundException()
        {
            _command.Id = int.MaxValue;

            var handler = new UpdateProductCommandHandler(Context, Mapper);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(_command, CancellationToken.None));
        }

        [Fact]
        public async Task GivenAlreadyUsedName_RaiseProductNameAlreadyInUseException()
        {
            _command.Name = TestContext.TestProduct2.Name;

            var handler = new UpdateProductCommandHandler(Context, Mapper);

            await Assert.ThrowsAsync<ProductNameAlreadyInUseException>(async () =>
                await handler.Handle(_command, CancellationToken.None));
        }
    }
}