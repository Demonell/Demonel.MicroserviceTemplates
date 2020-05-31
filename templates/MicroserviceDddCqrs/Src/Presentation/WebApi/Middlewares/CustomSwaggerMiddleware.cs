using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors.Security;
using WebApi.Filters;
using WebApi.Options;
using ZymLabs.NSwag.FluentValidation;

namespace WebApi.Middlewares
{
    public static class CustomSwaggerMiddleware
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, AppOptions appOptions)
        {
            services.AddSingleton<FluentValidationSchemaProcessor>();
            services.AddSwaggerDocument((configure, serviceProvider) =>
            {
                configure.Title = $"{appOptions.ApplicationName} HTTP API";
                configure.Version = $"v{appOptions.SwaggerVersion}";
                configure.Description = $"The {appOptions.ApplicationName} Service HTTP API";
                configure.GenerateEnumMappingDescription = true;

                var fluentValidationSchemaProcessor = serviceProvider.GetService<FluentValidationSchemaProcessor>();
                configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);

                configure.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = $"{appOptions.IdentityUrl}/connect/authorize",
                            TokenUrl = $"{appOptions.IdentityUrl}/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                {appOptions.ApplicationName.ToLower(), $"{appOptions.ApplicationName} API"}
                            }
                        }
                    }
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oauth2"));
                configure.OperationProcessors.Add(new AuthOperationProcessor());
                configure.OperationProcessors.Add(new JsonIgnoreQueryOperationFilter());
            });

            return services;
        }
    }

    public class AuthOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var declaringType = context.MethodInfo.DeclaringType;
            if (declaringType != null)
            {
                var authAttributes = declaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>();

                if (authAttributes.Any())
                {
                    context.OperationDescription.Operation.Responses.Add("401",
                        new OpenApiResponse {Description = "Ошибка авторизации"});

                    context.OperationDescription.Operation.Responses.Add("403",
                        new OpenApiResponse {Description = "Нет прав доступа к ресурсу"});
                }
            }

            return true;
        }
    }
}