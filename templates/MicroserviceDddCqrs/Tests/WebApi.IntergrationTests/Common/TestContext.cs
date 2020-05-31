using System;
using System.Collections.Generic;
using Application.Common.Interfaces;
using Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitTests.Common
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

        public Product TestProductCommon;
        public Product TestProductVip1;
        public Product TestProductVip2;

        private void InitializeDbForTests()
        {
            TestProductCommon = Context.Products.Add(new Product
            {
                Name = nameof(TestProductCommon),
                ProductType = ProductType.Common,
                Materials = new List<Material>
                {
                    new Material("wood", TimeSpan.FromDays(365)),
                    new Material("iron", TimeSpan.FromDays(4 * 365)),
                    new Material("aaa", TimeSpan.FromDays(4 * 365)),
                    new Material("xxx", TimeSpan.FromDays(4 * 365)),
                }
            }).Entity;

            TestProductVip1 = Context.Products.Add(new Product
            {
                Name = nameof(TestProductVip1),
                ProductType = ProductType.Vip,
                Materials = new List<Material>
                {
                    new Material("steel", TimeSpan.FromDays(16 * 365)),
                    new Material("fur", TimeSpan.FromDays(6 * 365)),
                    new Material("bbb", TimeSpan.FromDays(6 * 365)),
                    new Material("yyy", TimeSpan.FromDays(6 * 365)),
                }
            }).Entity;

            TestProductVip2 = Context.Products.Add(new Product
            {
                Name = nameof(TestProductVip2),
                ProductType = ProductType.Vip,
                Materials = new List<Material>
                {
                    new Material("obsidian", TimeSpan.FromDays(8 * 365)),
                    new Material("steel", TimeSpan.FromDays(16 * 365)),
                    new Material("ccc", TimeSpan.FromDays(16 * 365)),
                    new Material("zzz", TimeSpan.FromDays(16 * 365)),
                }
            }).Entity;

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}
