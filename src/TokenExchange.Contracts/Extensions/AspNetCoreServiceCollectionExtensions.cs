using Microsoft.Extensions.DependencyInjection;

namespace TokenExchange.Contracts.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddPrincipalEvaluatorRouter(this IServiceCollection services)
        {
            services.AddSingleton<IPrincipalEvaluatorRouter, PrincipalEvaluatorRouter>();
            return services;
        }
    }
}
