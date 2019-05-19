using IdentityTokenExchangeGraphQL.Query;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;
using TokenExchange.Contracts;
using TokenExchange.Contracts.Extensions;

namespace IdentityTokenExchangeGraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLIdentityTokenExchangeTypes(this IServiceCollection services)
        {
            services.AddSingleton<ITokenValidator, TokenValidator>();

            services.AddTokenExchangeContracts();

            // Bind Query

            services.AddTransient<HttpHeaderType>();
            services.AddTransient<TokenExchangeInput>();
            services.AddTransient<IdentityTokenInput>();
            services.AddTransient<TokenExchangeResponseType>();
            services.AddTransient<AccessTokenResponseType>();
            services.AddTransient<IdentityTokenResponseType>();
            services.AddTransient<CustomResponseType>();

            services.AddTransient<IQueryFieldRegistration, TokenExchangeQuery>();

        }
    }
}
