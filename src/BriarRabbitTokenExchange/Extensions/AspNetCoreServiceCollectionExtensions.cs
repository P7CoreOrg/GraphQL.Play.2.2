using BriarRabbitTokenExchange.Services;
using Microsoft.Extensions.DependencyInjection;
using P7Core.Extensions;
using TokenExchange.Contracts;

namespace BriarRabbitTokenExchange.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddBriarRabbitInProcTokenExchangeHandler(this IServiceCollection services)
        {
            services.AddLazyTransient<ITokenExchangeHandler, BriarRabbitInProcTokenExchangeHandler>();
            return services;
        }
    }
}
