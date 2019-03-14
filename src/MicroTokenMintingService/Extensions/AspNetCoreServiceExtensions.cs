using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts;

namespace TokenMintingService.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddMicroTokenMintingService(this IServiceCollection services) { 
            services.AddSingleton<ITokenMintingService, MicroTokenMintingService>();
        }
    }
}