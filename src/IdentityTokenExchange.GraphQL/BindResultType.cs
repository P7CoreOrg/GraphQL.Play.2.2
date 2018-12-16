using GraphQL.Types;
using IdentityTokenExchange.GraphQL.Models;

namespace IdentityTokenExchange.GraphQL
{
    public class BindResultType : ObjectGraphType<BindResultModel>
    {
        public BindResultType()
        {
            Name = "bindResult";
            Field<AuthorizationResultType>("authorization", "All pertinent information needed to make authorized calls");
        }
    }
}