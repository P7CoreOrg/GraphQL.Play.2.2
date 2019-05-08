using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace Self.Validator.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddSelfOIDCTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<ISelfValidator, SelfValidator>();
            services.AddSingleton<ISchemeTokenValidator, SelfOIDCTokenValidator>();
        }
    }
}
