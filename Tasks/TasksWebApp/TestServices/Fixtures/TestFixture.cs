using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TasksServices.Repository;
using TasksServices.Services;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Microsoft.Extensions.Logging;

namespace TestServices.Fixtures
{
   
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            services.AddTransient<ITodoItemService, TodoItemService>();
            services.AddTransient<ITodoItemRepository, TodoItemFileRepository>();
        }

        protected override ValueTask DisposeAsyncCore() => new ValueTask();
        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new TestAppSettings
            {
                Filename = "appsettings.json",
                IsOptional = false
            };
        }
        protected override ILoggingBuilder AddLoggingProvider(ILoggingBuilder loggingBuilder, ILoggerProvider loggerProvider)
        {
            return base.AddLoggingProvider(loggingBuilder, loggerProvider);
        }
    }
}