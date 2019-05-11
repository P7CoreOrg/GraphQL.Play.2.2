using GraphQLPlayTokenExchangeOnlyApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TestServerFixture;

namespace UnitTestProject_OAuth2
{
    public class MyTestServerFixture : TestServerFixture<Startup>
    {
        protected override string RelativePathToHostProject => @"..\..\..\..\GraphQLPlayTokenExchangeOnlyApp";
        protected override void ConfigureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            Program.LoadConfigurations(config, environmentName);
        }
    }
}