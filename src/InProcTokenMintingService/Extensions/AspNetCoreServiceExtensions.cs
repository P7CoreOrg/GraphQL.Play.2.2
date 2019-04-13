using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace TokenMintingService.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddInProcTokenMintingService(this IServiceCollection services) { 
            services.AddTransient<ITokenMintingService, InProcTokenMintingService>();
        }
    }
}