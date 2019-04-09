using IdentityModelExtras;
using IdentityModelExtras.Extensions;
using IdentityServer4ExtensionGrants.Rollup.Extensions;
using IdentityTokenExchangeGraphQL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using P7Core.GraphQLCore.Extensions;
using P7Core.GraphQLCore.Stores;
using System;
using TokenExchange.Contracts.Extensions;
using TokenMintingService.Extensions;

namespace GraphQLPlay.Rollup.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddGraphQLPlayRollup(this IServiceCollection services)
        {
            services.AddGraphQLCoreTypes();
            services.AddGraphQLIdentityTokenExchangeTypes();
            services.AddPrincipalEvaluatorRouter();
            services.AddInProcTokenMintingService();
            services.AddIdentityModelExtrasTypes();
            services.AddSingleton<DiscoverCacheContainerFactory>();
          

            return services;
        }
        public static IServiceCollection AddGraphQLPlayRollupInMemoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IGraphQLFieldAuthority, InMemoryGraphQLFieldAuthority>();
            services.RegisterGraphQLCoreConfigurationServices(configuration);
            services.AddExtensionGrantsRollup(configuration);
            return services;
        }
    }
}
