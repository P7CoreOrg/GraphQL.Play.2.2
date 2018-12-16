using GraphQL.Types;
using IdentityTokenExchange.GraphQL.Models;

namespace IdentityTokenExchange.GraphQL
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