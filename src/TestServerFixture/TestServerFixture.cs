using System;
using System.IO;
using System.Net.Http;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;

namespace TestServerFixture
{
    public abstract class TestServerFixture<TStartup> : 
        ITestServerFixture,
        IDisposable 
        where TStartup : class 
    {
    private readonly TestServer _testServer;
    public HttpClient Client { get; }
    public HttpMessageHandler MessageHandler { get; }
    public TestServer TestServer => _testServer;

    // RelativePathToHostProject = @"..\..\..\..\GraphQLPlayTokenExchangeOnlyApp";
    protected abstract string RelativePathToHostProject { get; }
    public TestServerFixture()
    {
        var contentRootPath = GetContentRootPath();
        var builder = new WebHostBuilder()
            .UseContentRoot(contentRootPath)
            .UseEnvironment("Development")
            .ConfigureServices(services => { services.TryAddTransient<IDefaultHttpClientFactory, TestDefaultHttpClientFactory>(); })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                LoadConfigurations(config, environmentName);

            })
            .UseStartup<TStartup>(); // Uses Start up class from your API Host project to configure the test server

        _testServer = new TestServer(builder);
        Client = _testServer.CreateClient();
        MessageHandler = _testServer.CreateHandler();

    }

    protected abstract void LoadConfigurations(IConfigurationBuilder config, string environmentName);

    private string GetContentRootPath()
    {
        var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
        return Path.Combine(testProjectPath, RelativePathToHostProject);
    }

    public void Dispose()
    {
        Client.Dispose();
        _testServer.Dispose();
    }
    }
}
