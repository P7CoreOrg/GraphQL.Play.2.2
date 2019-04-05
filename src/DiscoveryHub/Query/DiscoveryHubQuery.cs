using DiscoveryHub.Contracts;
using DiscoveryHub.Models;
using GraphQL;
using P7Core.GraphQLCore;
using System;

using System.Linq;


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
                resolve: async context =>
                {
                    try
                    {
                        var endpoints = await _discoveryHubStore.GetGraphQLEndpointsAsync();

                        return new DiscoveryResult()
                        {
                            GraphQLEndpoints = endpoints.ToList()
                        };
                    }
                    catch (Exception e)
                    {
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);
        }
    }
}
