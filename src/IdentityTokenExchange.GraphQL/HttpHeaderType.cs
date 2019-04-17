using GraphQL.Types;
using IdentityTokenExchangeGraphQL.Models;
using TokenExchange.Contracts.Models;

namespace IdentityTokenExchangeGraphQL
{
    public class HttpHeaderType : ObjectGraphType<HttpHeader>
    {
        public HttpHeaderType()
        {
            Field(x => x.Name).Description("The header name");
            Field(x => x.Value).Description("The header value.");
        }
    }
}