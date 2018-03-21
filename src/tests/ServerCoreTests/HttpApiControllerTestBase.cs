using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ServerCore;

namespace ServerCoreTests
{
    public abstract class HttpApiControllerTestBase : IDisposable
    {
        protected void Initialize(Action<IServiceCollection> configureServices)
        {
            var startup = new TestStartup(configureServices);
            var builder = WebHost.CreateDefaultBuilder()
                    // This is needed when using IStartup via DI - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/hosting?tabs=aspnetcore2x
                    .UseSetting("applicationName", Assembly.GetExecutingAssembly().FullName)
                    .ConfigureServices(s => s.AddSingleton<IStartup>(startup));
            Server = new TestServer(builder);
            HttpClient = Server.CreateClient();
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
            Server?.Dispose();
        }

        protected TestServer Server { get; private set; }

        protected HttpClient HttpClient { get; private set; }

        protected RequestBuilder CreateRequest(string uri)
        {
            return Server.CreateRequest(uri);
        }

        private class TestStartup : Startup
        {
            private readonly Action<IServiceCollection> _configureServices;

            public TestStartup(Action<IServiceCollection> configureServices)
            {
                _configureServices = configureServices;
            }

            public override IServiceProvider ConfigureServices(IServiceCollection services)
            {
                base.ConfigureServices(services);
                _configureServices(services);
                return services.BuildServiceProvider();
            }
        }
    }
}
