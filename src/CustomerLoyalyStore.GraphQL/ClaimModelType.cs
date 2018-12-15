using CustomerLoyalyStore.GraphQL.Models;
using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL
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