using GraphQL.Types;
using IdentityTokenExchangeGraphQL.Models;

namespace IdentityTokenExchangeGraphQL
{
    public class AuthorizationResultType : ObjectGraphType<AuthorizationResultModel>
    {
        public AuthorizationResultType()
        {
            Name = "authorizationResult";
            Field(x => x.access_token).Description("The access_token.");
            Field(x => x.expires_in).Description("Expired in seconds.");
            Field(x => x.token_type).Description("The type of token.");
            Field(x => x.refresh_token).Description("The refresh_token.");
            Field(x => x.authority).Description("The authority.");
            Field<ListGraphType<HttpHeaderType>>("HttpHeaders",
                "These additional headers must be included when making an authorized call.");
        }
    }
}