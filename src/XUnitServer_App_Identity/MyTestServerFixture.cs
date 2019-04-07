using IdentityServer4_Extension_Grants_App;
using Microsoft.Extensions.Configuration;
using XUnitHelpers;

namespace XUnit_IdentityServer4_Extension_Grants_App
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
