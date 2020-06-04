using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Products.Models
{
    /// <summary>
    /// Материал
    /// </summary>
    public class MaterialVm : IMapFrom<Material>, IMapTo<Material>
    {
        /// <summary>
        /// Наименование материала
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Срок службы материала
        /// </summary>
        public TimeSpan Durability { get; set; }
    }
}