using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Commands.CreateProduct;
using Application.Products.Models;
using Application.UnitTests.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.UnitTests.Products
{
    public class CreateProductCommandHandlerTests : TestBase
    {
        private readonly CreateProductCommandHandler _handler;
        private readonly CreateProductCommand _command;

        public CreateProductCommandHandlerTests()
        {
            _handler = new CreateProductCommandHandler(Context, Mapper);

            _command = new CreateProductCommand
            {
                Name = "test",
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
            var id = await _handler.Handle(_command, CancellationToken.None);

            var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == id);

            Assert.NotNull(product);
            Assert.Equal("test", product.Name);
            Assert.Equal(TestUserId, product.CreatedBy);
            Assert.Equal(DateTimeOffset.Now.Year, product.Created.Year);
        }

        [Fact]
        public async Task GivenNullMaterials_Pass()
        {
            _command.Materials = null;

            var id = await _handler.Handle(_command, CancellationToken.None);

            var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == id);

            Assert.NotNull(product);
            Assert.Empty(product.Materials);
        }

        [Fact]
        public async Task GivenAlreadyUsedName_RaiseProductNameAlreadyInUseException()
        {
            _command.Name = TestContext.TestProductCommon.Name;

            await Assert.ThrowsAsync<ProductNameAlreadyInUseException>(async () =>
                await _handler.Handle(_command, CancellationToken.None));
        }
    }
}