using Application.Products.Models;
using MediatR;

namespace Application.Products.Queries.GetProduct
{
    /// <summary>
    /// Запрос на получение продукта
    /// </summary>
    public class GetProductQuery : IRequest<ProductVm>
    {
        /// <summary>
        /// Уникальный идентификатор продукта
        /// </summary>
        public int Id { get; set; }
    }
}
