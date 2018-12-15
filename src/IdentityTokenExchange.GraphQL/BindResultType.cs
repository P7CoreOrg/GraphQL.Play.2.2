using GraphQL.Types;
using IdentityTokenExchange.GraphQL.Models;

namespace IdentityTokenExchange.GraphQL
{
    public class BindResultType : ObjectGraphType<BindResultModel>
    {
        public BindResultType()
        {
            Name = "bindResult";
            Field(x => x.access_token).Description("The access_token.");
            Field(x => x.expires_in).Description("Expired in seconds.");
            Field(x => x.token_type).Description("The type of token.");
            Field(x => x.refresh_token).Description("The refresh_token.");
            Field(x => x.authority).Description("The authority.");

        }
    }
}