using System;
using Application;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NSwag.AspNetCore;
using Persistence;
using WebApi.Filters;
using WebApi.HostedServices;
using WebApi.Middlewares;
using WebApi.Options;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppOptions>(Configuration.GetSection(nameof(AppOptions)));
            var appOptions = Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            ConfigureAuthService(services, appOptions);
            services.AddCustomCors(appOptions);
            services.AddCustomSwagger(appOptions);

            services.AddCustomMetrics(Configuration);
            services.AddCustomHealthCheck(Configuration);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddInfrastructure();
            services.AddPersistence(Configuration);
            services.AddApplication();

            services
                .AddControllers(o => o.Filters.Add(typeof(GlobalExceptionFilter)))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IAppDbContext>());

            services.AddHostedService<MigrationHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<AppOptions> appOptions)
        {
            Console.Title = env.ApplicationName;

            app.UseCustomMetrics();
            app.UseCustomHealthCheck("/hc");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3(c =>
            {
                c.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = appOptions.CurrentValue.ApplicationName.ToLower() + "_swaggerui",
                    AppName = "Swagger UI"
                };
            });

            app.UseRouting();

            app.UseCors();

            ConfigureAuth(app);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);
        }

        private void ConfigureAuthService(IServiceCollection services, AppOptions appOptions)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = appOptions.IdentityUrl;
                    options.Audience = appOptions.ApplicationName.ToLower();
                    options.RequireHttpsMetadata = false;
                });
        }

        private void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}