using GraphQLPlayTokenExchangeOnlyApp;
using Microsoft.Extensions.Configuration;
using TestServerFixture;

namespace UnitTestProject_App_Identity
{
    public class MyTestServerFixture : TestServerFixture<Startup>
    {
        protected override string RelativePathToHostProject => @"..\..\..\..\GraphQLPlayTokenExchangeOnlyApp";

        protected override void LoadConfigurations(IConfigurationBuilder config, string environmentName)
        {
            Program.LoadConfigurations(config, environmentName);
        }
    }
}