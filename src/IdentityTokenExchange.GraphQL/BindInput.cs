using GraphQL.Types;

namespace IdentityTokenExchangeGraphQL
{
    public class BindInput : InputObjectGraphType
    {
        /*
            token:blah
            tokenType:id_token
            authorityKey:wellknownAuthority
         */
        public BindInput()
        {
            Name = "bind";
            Field<NonNullGraphType<GraphQL.Types.StringGraphType>>("token");
            Field<NonNullGraphType<StringGraphType>>("tokenScheme");
            Field<ListGraphType<StringGraphType>>("extras");
        }
    }
}