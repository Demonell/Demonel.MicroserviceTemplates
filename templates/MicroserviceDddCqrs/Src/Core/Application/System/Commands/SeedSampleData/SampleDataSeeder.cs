using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.System.Commands.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IAppDbContext _context;

        public SampleDataSeeder(IAppDbContext context)
        {
            _context = context;
        }

        public async Task SeedData(CancellationToken cancellationToken)
        {
            await SeedProducts(cancellationToken);
        }

        public async Task SeedProducts(CancellationToken cancellationToken)
        {
            var anyProducts = await _context.Products.AnyAsync(cancellationToken);
            if (anyProducts)
            {
                _context.Products.AddRange(new List<Product>
                {
                    new Product
                    {
                        Name = "Sample Product Common",
                        ProductType = ProductType.Common,
                        DeliveryDate = DateTimeOffset.Now,
                        Materials = new List<Material>
                        {
                            new Material("wood", TimeSpan.FromDays(365 * 1)),
                            new Material("iron", TimeSpan.FromDays(365 * 4))
                        }
                    },
                    new Product
                    {
                        Name = "Sample Product Vip",
                        ProductType = ProductType.Vip,
                        DeliveryDate = DateTimeOffset.Now.AddDays(2),
                        Materials = new List<Material>
                        {
                            new Material("steel", TimeSpan.FromDays(365 * 16)),
                            new Material("cutton", TimeSpan.FromDays(365 * 2))
                        }
                    }
                });

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}