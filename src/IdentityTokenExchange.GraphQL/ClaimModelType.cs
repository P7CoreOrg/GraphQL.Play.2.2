using GraphQL.Types;
using IdentityTokenExchange.GraphQL.Models;

namespace IdentityTokenExchange.GraphQL
{
    public class ClaimModelType : ObjectGraphType<ClaimModel>
    {
        public ClaimModelType()
        {
            Name = "claim";

            Field(x => x.Name).Description("name of claim.");
            Field(x => x.Value).Description("value of claim.");
        }
    }
     
}