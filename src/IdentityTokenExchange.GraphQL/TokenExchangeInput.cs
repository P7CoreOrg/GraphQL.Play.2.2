using GraphQL.Types;

namespace IdentityTokenExchangeGraphQL
{
    public class TokenExchangeInput : InputObjectGraphType
    {
       
        public TokenExchangeInput()
        {
            Name = "tokenExchange";
            Field<ListGraphType<IdentityTokenInput>>("tokens");
            Field<NonNullGraphType<StringGraphType>>("exchange");
            Field<ListGraphType<StringGraphType>>("extras");
        }
    }
}