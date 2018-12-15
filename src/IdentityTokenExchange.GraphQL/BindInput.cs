using GraphQL.Types;

namespace IdentityTokenExchange.GraphQL
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
            Field<NonNullGraphType<StringGraphType>>("token");
            Field<NonNullGraphType<StringGraphType>>("tokenScheme");
            Field<NonNullGraphType<StringGraphType>>("authorityKey");
        }
    }
}