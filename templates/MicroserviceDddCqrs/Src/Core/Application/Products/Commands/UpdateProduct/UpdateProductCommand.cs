using System.Collections.Generic;
using Application.Common.Mappings;
using Application.Products.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.Products.Commands.UpdateProduct
{
    /// <summary>
    /// Команда на изменение параметров продукта
    /// </summary>
    public class UpdateProductCommand : IRequest, IMapTo<Product>
    {
        /// <summary>
        /// Уникальный идентификатор продукта
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

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
            profile.CreateMap<UpdateProductCommand, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .IgnoreAuditableProperties();
        }
    }
}
