using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts.Stores;

namespace TokenExchange.Contracts.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenExchangeContracts(this IServiceCollection services)
        {
            services.AddTransient<OIDCTokenValidator>();
            services.AddTransient<IPrincipalEvaluatorRouter, PrincipalEvaluatorRouter>();
            return services;
        }
       
        public static IServiceCollection AddDemoCustomIdentityPrincipalEvaluator(this IServiceCollection services)
        {
            services.AddTransient<IPrincipalEvaluator, AlienCustomIdentityPrincipalEvaluator>();
            services.AddTransient<IPrincipalEvaluator, GoogleMyCustomIdentityPrincipalEvaluator>();
            return services;
        }
        public static IServiceCollection AddSelfIdentityPrincipalEvaluator(this IServiceCollection services)
        {
            services.AddTransient<IPrincipalEvaluator, SelfIdentityPrincipalEvaluator>();
            return services;
        }
        public static IServiceCollection AddInMemoryExternalExchangeStore(this IServiceCollection services)
        {
            services.AddTransient<ExternalExchangePrincipalEvaluator>();
            services.AddSingleton<IExternalExchangeStore, InMemoryExternalExchangeStore>();
            return services;
        }
    }
}
