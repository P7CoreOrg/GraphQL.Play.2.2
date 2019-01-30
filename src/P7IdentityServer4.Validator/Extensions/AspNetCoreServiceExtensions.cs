using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace P7IdentityServer4.Validator.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddP7IdentityServer4OIDCTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<ISchemeTokenValidator, P7IdentityServer4OIDCTokenValidator>();
        }
    }
}
