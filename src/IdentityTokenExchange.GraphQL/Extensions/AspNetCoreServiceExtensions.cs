using IdentityTokenExchange.GraphQL.Query;
using IdentityTokenExchange.GraphQL.Services;
using Microsoft.Extensions.DependencyInjection;
using P7.GraphQLCore;

namespace IdentityTokenExchange.GraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLIdentityTokenExchangeTypes(this IServiceCollection services)
        {
            services.AddTransient<IOIDCTokenValidator, OIDCTokenValidator>();
            services.AddTransient<ITokenValidator, TokenValidator>();
          
            // AuthRequired Query
            services.AddTransient<IdentityModelType>();
            services.AddTransient<ClaimModelType>();
            services.AddTransient<IQueryFieldRecordRegistration, AuthRequiredQuery>();


            // Bind Query
            services.AddTransient<BindInput>();
            services.AddTransient<BindResultType>();
            services.AddTransient<IQueryFieldRecordRegistration, BindQuery>();

        }
    }
}
