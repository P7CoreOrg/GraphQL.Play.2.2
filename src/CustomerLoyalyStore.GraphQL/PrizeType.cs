using CustomerLoyaltyStore.Models;
using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL
{
    public class PrizeType : ObjectGraphType<Prize>
    {
        public PrizeType()
        {
            Name = "prize";
            Field(x => x.ID).Description("prize Id.");
            Field(x => x.LoyaltyPointsCost).Description("Prize loyalty point cost.");
        }
    }
}