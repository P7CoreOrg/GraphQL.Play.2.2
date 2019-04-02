using GraphQL.Types;

namespace IdentityTokenExchangeGraphQL
{
    public class BindInput : InputObjectGraphType
    {
       
        public BindInput()
        {
            Name = "bind";
            Field<NonNullGraphType<GraphQL.Types.StringGraphType>>("token");
            Field<NonNullGraphType<StringGraphType>>("tokenScheme");
            Field<NonNullGraphType<StringGraphType>>("exchange");
            Field<ListGraphType<StringGraphType>>("extras");
        }
    }
}