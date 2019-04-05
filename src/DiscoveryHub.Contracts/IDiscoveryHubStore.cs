using DiscoveryHub.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscoveryHub.Contracts
{
    public interface IDiscoveryHubStore
    {
        Task<IEnumerable<GraphQLEndpoint>> GetGraphQLEndpointsAsync();
    }
}
