using Microsoft.Extensions.DependencyInjection;
using WebApi.Options;

namespace WebApi.Middlewares
{
    public static class CustomCorsMiddleware
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, AppOptions appOptions)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(appOptions.AllowedCorsOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}