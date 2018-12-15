using IdentityTokenExchange.GraphQL.Query;
using Microsoft.Extensions.DependencyInjection;
using P7.GraphQLCore;

namespace IdentityTokenExchange.GraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLIdentityTokenExchangeTypes(this IServiceCollection services)
        {
            services.AddTransient<IdentityModelType>();
            services.AddTransient<ClaimModelType>();
          
            services.AddTransient<IQueryFieldRecordRegistration, AuthRequiredQuery>();

        }
    }
}
