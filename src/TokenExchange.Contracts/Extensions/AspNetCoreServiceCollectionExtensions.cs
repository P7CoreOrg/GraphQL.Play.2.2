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

        public static IServiceCollection AddGoogleIdentityPrincipalEvaluator(this IServiceCollection services)
        {

            services.AddSingleton<IPrincipalEvaluator, GoogleIdentityPrincipalEvaluator>();
            return services;
        }
        public static IServiceCollection AddGoogleMyCustomIdentityPrincipalEvaluator(this IServiceCollection services)
        {

            services.AddSingleton<IPrincipalEvaluator, GoogleMyCustomIdentityPrincipalEvaluator>();
            return services;
        }
        public static IServiceCollection AddSelfIdentityPrincipalEvaluator(this IServiceCollection services)
        {
            services.AddSingleton<IPrincipalEvaluator, SelfIdentityPrincipalEvaluator>();
            return services;
        }
    }
}
