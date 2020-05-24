using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Products.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductVm>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductVm> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                          ?? throw new NotFoundException($"Product with id: {request.Id} has not been found");

            var productVm = _mapper.Map<ProductVm>(product);

            return productVm;
        }
    }
}
