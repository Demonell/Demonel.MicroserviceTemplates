using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.System.Commands.SeedSampleData
{
    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly IAppDbContext _context;

        public SeedSampleDataCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            var dataSeeder = new SampleDataSeeder(_context);
            
            await dataSeeder.SeedData(cancellationToken);

            return Unit.Value;
        }
    }
}
