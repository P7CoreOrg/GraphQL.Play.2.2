using GraphQL.Types;


namespace DiscoveryHub.Models
{
    public class DiscoveryResultType : ObjectGraphType<DiscoveryResult>
    {
        public DiscoveryResultType()
        {
            Name = "discoveryResult";
            Field<ListGraphType<GraphQLEndpointType>>("graphQLEndpoints", "wellknown graphQL endpoints");
        }
    }
}