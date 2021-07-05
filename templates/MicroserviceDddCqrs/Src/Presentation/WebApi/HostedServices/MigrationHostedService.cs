using System;
using System.Threading;
using System.Threading.Tasks;
using Application.System.Commands.SeedSampleData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace WebApi.HostedServices
{
    public class MigrationHostedService : BackgroundService
    {
        private readonly ILogger<MigrationHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MigrationHostedService(ILogger<MigrationHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An errorVm occurred while migrating or initializing the database.");
                throw;
            }
        }
    }
}