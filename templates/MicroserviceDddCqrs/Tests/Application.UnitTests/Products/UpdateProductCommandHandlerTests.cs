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
        private readonly UpdateProductCommandHandler _handler;
        private readonly UpdateProductCommand _command;

        public UpdateProductCommandHandlerTests()
        {
            _handler = new UpdateProductCommandHandler(Context, Mapper);

            _command = new UpdateProductCommand
            {
                Id = TestContext.TestProductCommon.Id,
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
            await _handler.Handle(_command, CancellationToken.None);

            var product = await Context.Products.FirstOrDefaultAsync(u => u.Id == _command.Id);

            Assert.Equal(TestUserId, product.LastModifiedBy);
            Assert.Equal(DateTimeOffset.Now.Year, product.LastModified?.Year);
        }

        [Fact]
        public async Task GivenSameName_Pass()
        {
            _command.Name = TestContext.TestProductCommon.Name;

            await _handler.Handle(_command, CancellationToken.None);
        }

        [Fact]
        public async Task GivenNonExistedId_RaiseNotFoundException()
        {
            _command.Id = int.MaxValue;

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _handler.Handle(_command, CancellationToken.None));
        }

        [Fact]
        public async Task GivenAlreadyUsedName_RaiseProductNameAlreadyInUseException()
        {
            _command.Name = TestContext.TestProductVip1.Name;

            await Assert.ThrowsAsync<ProductNameAlreadyInUseException>(async () =>
                await _handler.Handle(_command, CancellationToken.None));
        }
    }
}