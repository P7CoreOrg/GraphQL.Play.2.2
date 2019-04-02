using GraphQL.Types;
using IdentityTokenExchangeGraphQL.Models;

namespace IdentityTokenExchangeGraphQL
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