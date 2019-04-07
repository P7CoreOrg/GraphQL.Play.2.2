using IdentityServer4_Extension_Grants_App;
using Microsoft.Extensions.Configuration;
using XUnitHelpers;

namespace XUnitTestServerBase
{

    public class MyTestServerFixture : TestServerFixture<Startup>
    {
        protected override string RelativePathToHostProject => @"..\..\..\..\IdentityServer4-Extension-Grants-App";

        protected override void LoadConfigurations(IConfigurationBuilder config, string environmentName)
        {
            Program.LoadConfigurations(config, environmentName);
        }
    }
}
