using GraphQL.Types;
using IdentityTokenExchangeGraphQL.Models;
using TokenExchange.Contracts;

namespace IdentityTokenExchangeGraphQL
{
    public class TokenExchangeResponsesType : ListGraphType<TokenExchangeResponseType> { }
    public class TokenExchangeResponseType : ObjectGraphType<TokenExchangeResponse>
    {
        public TokenExchangeResponseType()
        {
            Name = "tokenExchangeResponse";
            Field<AccessTokenResponseType>("accessToken", "OIDC Type Token.");
            Field<IdentityTokenResponseType>("identityToken", "OIDC Type Token.");
            Field<CustomResponseType>("customToken", "Custom Type Token.");
        }
    }
    public class CustomResponseType : ObjectGraphType<CustomTokenResponse>
    {
        public CustomResponseType()
        {
            Name = "customResponseType";
            Field(x => x.hint).Description("The hint.");
            Field(x => x.Token).Description("The custom token.");
            Field(x => x.Type, true).Description("The type of token.");
            Field(x => x.authority, true).Description("The authority.");
            Field<ListGraphType<HttpHeaderType>>("HttpHeaders",
                "These additional headers must be included when making an authorized call.");
        }
    }
    public class AccessTokenResponseType : ObjectGraphType<AccessTokenResponse>
    {
        public AccessTokenResponseType()
        {
            Name = "accessTokenResponseType";
            Field(x => x.hint).Description("The hint.");
            Field(x => x.access_token, true).Description("The access_token.");
            Field(x => x.expires_in).Description("Expired in seconds.");
            Field(x => x.token_type).Description("The type of token.");
            Field(x => x.refresh_token, true).Description("The refresh_token.");
            Field(x => x.authority).Description("The authority.");
            Field<ListGraphType<HttpHeaderType>>("HttpHeaders",
                "These additional headers must be included when making an authorized call.");
        }
    }
    public class IdentityTokenResponseType : ObjectGraphType<IdentityTokenResponse>
    {
        public IdentityTokenResponseType()
        {
            Name = "identityTokenResponseType";
            Field(x => x.hint).Description("The hint.");
            Field(x => x.id_token, true).Description("The id_token.");
            Field(x => x.expires_in).Description("Expired in seconds.");
            Field(x => x.authority).Description("The authority.");
            Field<ListGraphType<HttpHeaderType>>("HttpHeaders",
                "These additional headers must be included when making an authorized call.");
        }
    }
}