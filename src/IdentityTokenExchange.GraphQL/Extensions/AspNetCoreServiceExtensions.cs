using IdentityTokenExchange.GraphQL.Query; 
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;
using TokenExchange.Contracts;

namespace IdentityTokenExchange.GraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLIdentityTokenExchangeTypes(this IServiceCollection services)
        {
            services.AddSingleton<ITokenValidator, TokenValidator>();
            // AuthRequired Query
            services.AddTransient<IdentityModelType>();
            services.AddTransient<ClaimModelType>();
            services.AddTransient<IQueryFieldRegistration, AuthRequiredQuery>();


            // Bind Query
            
            services.AddTransient<HttpHeaderType>();
            services.AddTransient<BindInput>();
            services.AddTransient<BindResultType>();
            services.AddTransient<AuthorizationResultType>();
            services.AddTransient<IQueryFieldRegistration, BindQuery>();

        }
    }
}
