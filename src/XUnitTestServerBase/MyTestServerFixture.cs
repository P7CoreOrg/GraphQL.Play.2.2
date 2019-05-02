
using GraphQLPlayTokenExchangeOnlyApp;
using Microsoft.Extensions.Configuration;
using XUnitHelpers;

namespace XUnitTestServerBase
{

    public class MyTestServerFixture : TestServerFixture<Startup>
    {
        protected override string RelativePathToHostProject => @"..\..\..\..\..\GraphQLPlayTokenExchangeOnlyApp";

        protected override void LoadConfigurations(IConfigurationBuilder config, string environmentName)
        {
            Program.LoadConfigurations(config, environmentName);
        }
    }
}
