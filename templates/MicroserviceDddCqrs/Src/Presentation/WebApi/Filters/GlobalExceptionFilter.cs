using System.Net;
using Application.Common.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            switch (exception)
            {
                case FieldsValidationException fieldsValidationException:
                    _logger.LogWarning(exception.Message);
                    var problemDetails = new ValidationProblemDetails(fieldsValidationException.Failures)
                    {
                        Title = fieldsValidationException.Message,
                        Status = (int) HttpStatusCode.BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.ExceptionHandled = true;
                    break;

                case DomainException _:
                case AppException _:
                    _logger.LogWarning(exception.Message);
                    var apiError = new ApiErrorVm(exception);
                    context.Result = new UnprocessableEntityObjectResult(apiError);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    context.ExceptionHandled = true;
                    break;

                case NotFoundException _:
                    _logger.LogWarning(exception.Message);
                    var notFoundError = new ApiErrorVm(exception);
                    context.Result = new NotFoundObjectResult(notFoundError);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}