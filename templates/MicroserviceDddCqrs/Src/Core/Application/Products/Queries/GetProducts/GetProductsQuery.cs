using Application.Common.Models;
using Application.Products.Models;
using Domain.Entities;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : SortablePageable, IRequest<TotalList<ProductVm>>
    {
        /// <summary>
        /// Уникальный идентификатор продукта
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип продукта
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// Период дат доставки
        /// </summary>
        public DateRange DeliveryDate { get; set; }

        /// <summary>
        /// Наименование материала
        /// </summary>
        public string MaterialName { get; set; }
    }
}
