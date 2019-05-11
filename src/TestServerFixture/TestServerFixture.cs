using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using TestServerFixture;

namespace TestServerFixture
{
    public abstract class TestServerFixture<TStartup> :
        ITestServerFixture
        where TStartup : class
    {
        private readonly TestServer _testServer;
        private string _environmentUrl;
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
                .ConfigureServices(services =>
                {
                    services.PostConfigure<JwtBearerOptions>("Bearer-self-testserver", options =>
                    {
                        options.BackchannelHttpHandler = MessageHandler;
                    });

                    services.TryAddTransient<IDefaultHttpClientFactory>(serviceProvider =>
                    {
                        return new TestDefaultHttpClientFactory()
                        {
                            HttpClient = Client,
                            HttpMessageHandler = MessageHandler
                        };
                    });
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    LoadConfigurations(config, environmentName);

                })
                .UseStartup<TStartup>(); // Uses Start up class from your API Host project to configure the test server
            string environmentUrl = Environment.GetEnvironmentVariable("TestEnvironmentUrl");
            IsUsingInProcTestServer = false;
            if (string.IsNullOrWhiteSpace(environmentUrl))
            {
                environmentUrl = "http://localhost/";

                _testServer = new TestServer(builder);

                MessageHandler = _testServer.CreateHandler();
                IsUsingInProcTestServer = true;

                // We need to suppress the execution context because there is no boundary between the client and server while using TestServer
                MessageHandler = new SuppressExecutionContextHandler(MessageHandler);
            }

            else
            {
                if (environmentUrl.Last() != '/')
                {
                    environmentUrl = $"{environmentUrl}/";
                }

                MessageHandler = new HttpClientHandler();
            }


            _environmentUrl = environmentUrl;

        }

        public bool IsUsingInProcTestServer { get; set; }
        public HttpClient CreateHttpClient()
            => new HttpClient(new SessionMessageHandler(MessageHandler)) { BaseAddress = new Uri(_environmentUrl) };
        public HttpClient Client => CreateHttpClient();
        protected abstract void LoadConfigurations(IConfigurationBuilder config, string environmentName);

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            return Path.Combine(testProjectPath, RelativePathToHostProject);
        }

    }
}
