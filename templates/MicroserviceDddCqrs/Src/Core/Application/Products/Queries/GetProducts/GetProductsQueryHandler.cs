using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Products.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, TotalList<ProductVm>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TotalList<ProductVm>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products.AsQueryable();

            query = Filter(query, request);
            var total = await query.CountAsync(cancellationToken);
            query = Sort(query, request.Sorts);
            query = SkipTake(query, request);

            var products = await query.ToListAsync(cancellationToken);
            var productsVm = _mapper.Map<List<ProductVm>>(products);

            return new TotalList<ProductVm>(productsVm, total);
        }

        private IQueryable<Product> Filter(IQueryable<Product> products, GetProductsQuery request)
        {
            return products.Where(p =>
                (request.Id == null || request.Id == p.Id)
                && (string.IsNullOrEmpty(request.Name) || EF.Functions.ILike(p.Name, $"%{request.Name}%"))
                && (request.ProductType == null || request.ProductType == p.ProductType)
                && (request.MaterialName == null ||
                    p.Materials.Any(m => EF.Functions.ILike(m.Name, $"%{request.MaterialName}%"))));
        }

        private IQueryable<Product> Sort(IQueryable<Product> products, List<Sort> sorts)
        {
            foreach (var sort in sorts)
            {
                if (nameof(GetProductsQuery.Id).Equals(sort.Field, StringComparison.InvariantCultureIgnoreCase))
                    products = sort.Order(products, p => p.Id);
                else if (nameof(GetProductsQuery.Name).Equals(sort.Field, StringComparison.InvariantCultureIgnoreCase))
                    products = sort.Order(products, p => p.Name);
                else if (nameof(GetProductsQuery.ProductType).Equals(sort.Field, StringComparison.InvariantCultureIgnoreCase))
                    products = sort.Order(products, p => p.ProductType);
                else if (nameof(GetProductsQuery.MaterialName).Equals(sort.Field, StringComparison.InvariantCultureIgnoreCase))
                    products = sort.Order(products, p =>
                        (sort.Descending
                            ? p.Materials.OrderByDescending(m => m.Name)
                            : p.Materials.OrderBy(m => m.Name))
                        .Select(m => m.Name)
                        .FirstOrDefault());
                else
                    throw new FieldsValidationException(new List<ValidationFailure>
                    {
                        new ValidationFailure(nameof(Sortable.Sort), $"You can not sort by field: {sort.Field}")
                    });
            }

            return products;
        }

        private IQueryable<Product> SkipTake(IQueryable<Product> products, IPageable request)
        {
            if (request.Skip != null)
                products = products.Skip(request.Skip.Value);

            if (request.Take != null)
                products = products.Take(request.Take.Value);

            return products;
        }
    }
}