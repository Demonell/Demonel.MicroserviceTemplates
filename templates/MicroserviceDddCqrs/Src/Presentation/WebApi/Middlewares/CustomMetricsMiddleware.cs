using System.Diagnostics;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Gauge;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using App.Metrics.Health.Formatters.Ascii;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Middlewares
{
    public static class CustomMetricsMiddleware
    {
        public static IWebHostBuilder ConfigureCustomAppMetrics(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureAppMetricsHostingConfiguration(options =>
                {
                    options.AllEndpointsPort = 1111;
                    options.EnvironmentInfoEndpoint = "/env";
                    options.MetricsEndpoint = "/metrics-buf";
                    options.MetricsTextEndpoint = "/metrics";
                })
                .ConfigureAppHealthHostingConfiguration(options =>
                {
                    options.HealthEndpoint = "/ready";
                    options.PingEndpoint = "/liveness";
                });
        }

        public static IServiceCollection AddCustomMetrics(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            var metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsJson()
                .OutputMetrics.AsPrometheusPlainText()
                .Configuration.ReadFrom(configuration)
                .Build();

            var processMemoryGauge = new GaugeOptions
            {
                Name = "Process Memory",
                MeasurementUnit = Unit.Bytes,
            };

            metrics.Measure.Gauge.SetValue(processMemoryGauge, new MetricTags("type", "physical"),
                () => Process.GetCurrentProcess().WorkingSet64);
            metrics.Measure.Gauge.SetValue(processMemoryGauge, new MetricTags("type", "private"),
                () => Process.GetCurrentProcess().PrivateMemorySize64);
            metrics.Measure.Gauge.SetValue(processMemoryGauge, new MetricTags("type", "virtual"),
                () => Process.GetCurrentProcess().VirtualMemorySize64);

            services.AddMvcCore().AddMetricsCore();
            services.AddMetrics(metrics);
            services.AddMetricsReportingHostedService();
            services.AddMetricsTrackingMiddleware(configuration);

            services.AddMetricsEndpoints(
                options => { options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter(); },
                configuration);


            AppMetricsHealth.CreateDefaultBuilder()
                .OutputHealth.AsJson()
                .HealthChecks.RegisterFromAssembly(services)
                .Configuration.ReadFrom(configuration)
                .Report.ToMetrics(metrics)
                .BuildAndAddTo(services);

            services.AddHealthEndpoints(
                options => { options.HealthEndpointOutputFormatter = new HealthStatusTextOutputFormatter(); },
                configuration);
            services.AddHealthReportingHostedService();

            return services;
        }

        public static IApplicationBuilder UseCustomMetrics(this IApplicationBuilder app)
        {
            return app.UseMetricsAllMiddleware()
                .UseMetricsApdexTrackingMiddleware()
                .UseMetricsAllEndpoints()
                .UseHealthAllEndpoints();
        }
    }
}