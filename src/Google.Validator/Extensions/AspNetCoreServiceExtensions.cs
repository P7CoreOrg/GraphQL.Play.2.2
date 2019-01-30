using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace Google.Validator.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGoogleOIDCTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<ISchemeTokenValidator, GoogleOIDCTokenValidator>();
        }
    }
}
