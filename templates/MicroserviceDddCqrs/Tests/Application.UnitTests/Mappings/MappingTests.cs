using System;
using System.Collections.Generic;
using System.Linq;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Models;
using AutoMapper;
using Domain.Entities;
using Xunit;

namespace Application.UnitTests.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapMaterial_ToMaterialVm()
        {
            var entity = new Material("wood", TimeSpan.FromDays(2 * 365));

            var result = _mapper.Map<MaterialVm>(entity);

            Assert.NotNull(result);
            Assert.IsType<MaterialVm>(result);
            Assert.Equal("wood", result.Name);
            Assert.Equal(TimeSpan.FromDays(2 * 365), result.Durability);
        }

        [Fact]
        public void ShouldMapProduct_ToProductVm()
        {
            var entity = new Product
            {
                Name = "Test",
                ProductType = ProductType.Vip,
                Materials = new List<Material>
                {
                    new Material("cutton", TimeSpan.FromDays(365)),
                    new Material("iron", TimeSpan.FromDays(4 * 365))
                }
            };

            var result = _mapper.Map<ProductVm>(entity);

            Assert.NotNull(result);
            Assert.IsType<ProductVm>(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(ProductType.Vip, result.ProductType);
            Assert.NotEmpty(result.Materials);
            Assert.NotEmpty(result.Materials.First().Name);
        }

        [Fact]
        public void ShouldMapCreateProductCommand_ToProduct()
        {
            var entity = new CreateProductCommand
            {
                Name = "Test",
                ProductType = ProductType.Vip,
                Materials = new List<MaterialVm>
                {
                    new MaterialVm {Name = "cutton", Durability = TimeSpan.FromDays(365)},
                    new MaterialVm {Name = "cironutton", Durability = TimeSpan.FromDays(4 * 365)}
                }
            };

            var result = _mapper.Map<Product>(entity);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(ProductType.Vip, result.ProductType);
            Assert.Null(result.CreatedBy);
            Assert.NotEmpty(result.Materials);
            Assert.NotEmpty(result.Materials.First().Name);
        }


        [Fact]
        public void ShouldMapUpdateProductCommand_ToProduct()
        {
            var entity = new UpdateProductCommand
            {
                Id = 444,
                Name = "Test",
                ProductType = ProductType.Vip,
                Materials = new List<MaterialVm>
                {
                    new MaterialVm {Name = "cutton", Durability = TimeSpan.FromDays(365)},
                    new MaterialVm {Name = "cironutton", Durability = TimeSpan.FromDays(4 * 365)}
                }
            };

            var result = new Product
            {
                Id = 123
            };

            _mapper.Map(entity, result);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(123, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal(ProductType.Vip, result.ProductType);
            Assert.Null(result.CreatedBy);
            Assert.NotEmpty(result.Materials);
            Assert.NotEmpty(result.Materials.First().Name);
        }
    }
}