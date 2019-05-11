using DiscoveryHub.Contracts;
using DiscoveryHub.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;

using System.Linq;
using System.Threading.Tasks;

namespace DiscoveryHub.Query
{
    public class DiscoveryHubQuery : IQueryFieldRegistration
    {

        private IDiscoveryHubStore _discoveryHubStore;

        public DiscoveryHubQuery(IDiscoveryHubStore discoveryHubStore)
        {
            _discoveryHubStore = discoveryHubStore;

        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<DiscoveryResultType>(name: "graphQLDiscovery",
                description: $"Discovery of downstream graphQL services",
                resolve: GraphQLDiscoveryResolver,

                deprecationReason: null);
        }

        internal async Task<object> GraphQLDiscoveryResolver(ResolveFieldContext<object> context)
        {
            var endpoints = await _discoveryHubStore.GetGraphQLEndpointsAsync();
            return new DiscoveryResult()
            {
                GraphQLEndpoints = endpoints.ToList()
            };
        }
    }
}
