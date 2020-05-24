using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                          ?? throw new NotFoundException($"Product with id: {request.Id} has not been found");

            if (product.Name != request.Name)
            {
                var anyOtherProductWithSameName = await _context.Products
                    .AnyAsync(p => p.Name == request.Name && p.Id != request.Id, cancellationToken);
                if (anyOtherProductWithSameName)
                    throw new ProductNameAlreadyInUseException($"Product with name: {request.Name} already exist");
            }

            _mapper.Map(request, product);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
