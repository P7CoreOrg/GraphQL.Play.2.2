using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace Norton.Validator.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddNortonOIDCTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<ISchemeTokenValidator, NortonOIDCTokenValidator>();
        }
    }
}
