using System.Collections.Generic;
using Application.Common.Mappings;
using Application.Products.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    /// <summary>
    /// Команда создать продукт
    /// </summary>
    public class CreateProductCommand : IRequest<int>, IMapTo<Product>
    {
        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип продукта
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Материалы
        /// </summary>
        public List<MaterialVm> Materials { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProductCommand, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .IgnoreAuditableProperties();
        }
    }
}