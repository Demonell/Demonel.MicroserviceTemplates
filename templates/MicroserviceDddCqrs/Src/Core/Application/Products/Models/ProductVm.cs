using System.Collections.Generic;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Products.Models
{
    /// <summary>
    /// Продукт
    /// </summary>
    public class ProductVm : IMapFrom<Product>
    {
        /// <summary>
        /// Уникальный идентификатор продукта
        /// </summary>
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
    }
}