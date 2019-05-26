using System;
using AppIdentity.Extensions;
using AppIdentity.Models;
using GraphQLPlayTokenExchangeOnlyApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestServerFixture;

namespace XUnitServer_App_Identity
{
    public class MyTestServerFixture : TestServerFixture<Startup>
    {
        protected override string RelativePathToHostProject => @"../../../../GraphQLPlayTokenExchangeOnlyApp";
        protected override void ConfigureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            Program.LoadConfigurations(config, environmentName);
            config.AddJsonFile($"appsettings.TestServer.json", optional: false);
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigure<JwtBearerOptions>("Bearer-self-testserver", options =>
            {
                options.BackchannelHttpHandler = MessageHandler;
            });
            services.AddInMemoryAppIdentityConfiguration(new AppIdentityConfigurationModel()
            {
                MaxAppIdLength = Guid.NewGuid().ToString().Length,
                MaxMachineIdLength = Guid.NewGuid().ToString().Length,
                MaxSubjectLength = Guid.NewGuid().ToString().Length
            });
        }
    }
}