using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace DemoIdentityServerio.Validator.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddDemoIdentityServerioOIDCTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<ISchemeTokenValidator, DemoIdentityServerioOIDCTokenValidator>();
        }
    }
}
