﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected T Created<T>(T response)
        {
            HttpContext.Response.StatusCode = 201;
            return response;
        }
    }
}