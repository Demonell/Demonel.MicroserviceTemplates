using System.Threading.Tasks;
using Application.Common.Models;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Models;
using Application.Products.Queries.GetProduct;
using Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        /// <summary>
        /// Получение информации о продукте
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/products
        ///
        /// </remarks>
        /// <response code="200">Успех! Возвращает объект с информацией о продукте</response>
        /// <response code="400">Ошибка валидации полей</response> 
        /// <response code="404">Объект не найден</response> 
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<TotalList<ProductVm>> Get([FromQuery] GetProductsQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Получение информации о продукте
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/products/1
        ///
        /// </remarks>
        /// <param name="id">Уникальный идентификатор продукта</param>
        /// <response code="200">Успех! Возвращает объект с информацией о продукте</response>
        /// <response code="400">Ошибка валидации полей</response> 
        /// <response code="404">Объект не найден</response> 
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorVm), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProductVm> Get(int id)
        {
            return await Mediator.Send(new GetProductQuery {Id = id});
        }

        /// <summary>
        /// Создание продукта
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /api/products/create
        ///     {
        ///         "name": "my product",
        ///         "productType": 1,
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Успех! Возвращает уникальный идентификатор созданного продукта</response>
        /// <response code="400">Ошибка валидации полей</response> 
        /// <response code="422">Ошибка обработки запроса
        /// <para>Возможные коды ответа:</para>
        /// <para>ProductNameAlreadyInUseException - имя продукта уже используется</para>
        /// </response> 
        /// <response code="500">Внутренняя ошибка</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorVm), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<int> Create([FromBody] CreateProductCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /api/products/1
        ///     {
        ///         "name": "new product name",
        ///         "productType": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Уникальный идентификатор продукта</param>
        /// <param name="command">Объект новых параметров продукта</param>
        /// <response code="200">Успех</response>
        /// <response code="400">Ошибка валидации полей</response> 
        /// <response code="404">Объект не найден</response> 
        /// <response code="422">Ошибка обработки запроса
        /// <para>Возможные коды ответа:</para>
        /// <para>ProductNameAlreadyInUseException - имя продукта уже используется</para>
        /// </response> 
        /// <response code="500">Внутренняя ошибка</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorVm), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorVm), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;

            await Mediator.Send(command);

            return Ok();
        }
    }
}