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

namespace TokenExchange.Rollup.Extensions
{
    public static class AspNetCoreExtensions
    {
        public interface ITokenExchangeRegistrations
        {
            void AddTokenValidators(IServiceCollection services);
        }
        public static IServiceCollection AddTokenExchangeRollup(
            this IServiceCollection services, ITokenExchangeRegistrations tokenExchangeRegistrations)
        {
            services.AddGraphQLIdentityTokenExchangeTypes();
            services.AddInProcTokenMintingService();
            services.AddSingleton<DiscoverCacheContainerFactory>();
            tokenExchangeRegistrations.AddTokenValidators(services);
            return services;
        }
    }
}
