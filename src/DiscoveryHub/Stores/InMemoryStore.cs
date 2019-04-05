using DiscoveryHub.Contracts;
using DiscoveryHub.Contracts.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryHub.Stores
{
    public class InMemoryDiscoveryHubStore: IDiscoveryHubStore
    {
        private IConfiguration _configuration;

        List<GraphQLEndpoint> _graphQLEndpoints;
        List<GraphQLEndpoint> GraphQLEndpoints
        {
            get
            {
                if(_graphQLEndpoints == null)
                {
                    IConfigurationSection section = _configuration.GetSection("graphQLDiscoveryHub:endpoints");
                    _graphQLEndpoints = new List<GraphQLEndpoint>();
                    section.Bind(_graphQLEndpoints);
                }
                return _graphQLEndpoints;
            }
        }
            
        public InMemoryDiscoveryHubStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         
        public async Task<IEnumerable<GraphQLEndpoint>> GetGraphQLEndpointsAsync()
        {
            return GraphQLEndpoints;
        }
    }
}
