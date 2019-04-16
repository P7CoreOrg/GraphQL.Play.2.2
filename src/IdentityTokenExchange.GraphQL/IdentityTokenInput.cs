using GraphQL.Types;

namespace IdentityTokenExchangeGraphQL
{
    public class IdentityTokenInput : InputObjectGraphType
    {
        public IdentityTokenInput()
        {
            Name = "identity_token";
            Field<NonNullGraphType<GraphQL.Types.StringGraphType>>("token");
            Field<NonNullGraphType<StringGraphType>>("tokenScheme");
        }
    }
}