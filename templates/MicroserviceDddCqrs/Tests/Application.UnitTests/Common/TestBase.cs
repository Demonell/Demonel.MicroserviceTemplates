using System;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Common;
using Moq;
using Persistence;

namespace Application.UnitTests.Common
{
    public class TestBase
    {
        protected const string TestUserId = "1";

        protected TestContext TestContext;
        protected AppDbContext Context => TestContext.Context;
        protected IMapper Mapper { get; }
        protected Mock<ICurrentUserService> CurrentUserServiceMock { get; }
        protected Mock<IDateTime> DateTimeMock { get; }

        public TestBase()
        {
            var configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            Mapper = configurationProvider.CreateMapper();

            CurrentUserServiceMock = new Mock<ICurrentUserService>();
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(TestUserId);
            CurrentUserServiceMock.Setup(m => m.IsAuthenticated).Returns(true);

            DateTimeMock = new Mock<IDateTime>();
            DateTimeMock.Setup(m => m.Now).Returns(DateTime.Now);

            TestContext = new TestContext(CurrentUserServiceMock.Object, DateTimeMock.Object);
        }
    }
}