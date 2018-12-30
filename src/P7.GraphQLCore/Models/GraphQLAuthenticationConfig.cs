namespace P7.GraphQLCore.Models
{
    public class GraphQLAuthenticationConfig
    {
        public const string WellKnown_SectionName = "graphQLAuthentication";
        public MutationConfig Mutation { get; set; }
        public QueryConfig Query { get; set; }
    }
}