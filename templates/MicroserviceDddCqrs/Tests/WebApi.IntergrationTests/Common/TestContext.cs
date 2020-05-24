using System;
using System.Collections.Generic;
using Application.Common.Interfaces;
using Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace WebApi.IntergrationTests.Common
{
    public class TestContext : IDisposable
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public readonly string DatabaseName = Guid.NewGuid().ToString();
        public AppDbContext Context { get; }

        public TestContext(ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            Context = CreateDbContext();
            InitializeDbForTests();

            // to reload cache - all tracked entities (and included) will reset
            Context = CreateDbContext();
        }

        public TestContext(AppDbContext context)
        {
            Context = context;
            InitializeDbForTests();
        }

        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(DatabaseName)
                .Options;

            var context = new AppDbContext(options, _currentUserService, _dateTime);

            context.Database.EnsureCreated();

            return context;
        }

        public Product TestProduct1;
        public Product TestProduct2;

        private void InitializeDbForTests()
        {
            TestProduct1 = new Product
            {
                Name = nameof(TestProduct1),
                ProductType = ProductType.Common,
                Materials = new List<Material>
                {
                    new Material("wood", TimeSpan.FromDays(365)),
                    new Material("iron", TimeSpan.FromDays(4 * 365)),
                }
            };
            Context.Products.Add(TestProduct1);

            TestProduct2 = new Product
            {
                Name = nameof(TestProduct2),
                ProductType = ProductType.Vip,
                Materials = new List<Material>
                {
                    new Material("steel", TimeSpan.FromDays(16 * 365)),
                    new Material("cutton", TimeSpan.FromDays(6 * 365)),
                }
            };
            Context.Products.Add(TestProduct2);

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}
