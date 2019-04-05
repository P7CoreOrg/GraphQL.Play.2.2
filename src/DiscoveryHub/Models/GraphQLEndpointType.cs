using DiscoveryHub.Contracts.Models;
using DiscoveryHub.Models;
using GraphQL.Types;


namespace DiscoveryHub.Models
{
    public class GraphQLEndpointType : ObjectGraphType<GraphQLEndpoint>
    {
        public GraphQLEndpointType()
        {
            Name = "graphQLEndpoint";
            Field<StringGraphType>("url", "enpoint url");
            Field<StringGraphType>("name", "logical name of the enpoint");
        }
    }
}