using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace WebApi.IntergrationTests.Common
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
            => new XUnitLogger(_testOutputHelper, categoryName);

        public void Dispose()
        {
        }
    }

    public static class XunitLoggerProviderExtensions
    {
        public static ILoggingBuilder AddXUnitLogger(this ILoggingBuilder builder, ITestOutputHelper output)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(output));
            return builder;
        }
    }
}