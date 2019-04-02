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

        public static IServiceCollection AddGoogleUserPrincipalEvaluator(this IServiceCollection services)
        {

            services.AddSingleton<IPrincipalEvaluator, GoogleIdentityPrincipalEvaluator>();
            return services;
        }

        public static IServiceCollection AddSelfUserPrincipalEvaluator(this IServiceCollection services)
        {
            services.AddSingleton<IPrincipalEvaluator, SelfIdentityPrincipalEvaluator>();
            return services;
        }
    }
}
