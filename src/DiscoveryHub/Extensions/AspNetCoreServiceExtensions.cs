using DiscoveryHub.Contracts;
using DiscoveryHub.Models;
using DiscoveryHub.Query;
using DiscoveryHub.Stores;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;


namespace DiscoveryHub.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLDiscoveryTypes(this IServiceCollection services)
        {

            //GraphQL Discovery Query

            services.AddTransient<DiscoveryResultType>();
            services.AddTransient<GraphQLEndpointType>();
         

            services.AddTransient<IQueryFieldRegistration, DiscoveryHubQuery>();

        }
        public static void AddInMemoryDiscoveryHubStore(this IServiceCollection services)
        {
 

            services.AddSingleton<IDiscoveryHubStore, InMemoryDiscoveryHubStore>();

        }
    }
}
 