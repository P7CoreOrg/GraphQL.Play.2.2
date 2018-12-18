using System;
using CustomerLoyaltyStore;
using CustomerLoyaltyStore.Models;
using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL
{
    public class CustomerResultType : ObjectGraphType<CustomerResult>
    {
        public CustomerResultType()
        {
            Name = "customer";
            Field(x => x.ID).Description("customer Id.");
            Field(x => x.LoyaltyPointBalance).Description("Customers loyalty points balance.");
            Field<ListGraphType<PrizeType>>("prizes", "Loyalty Prize available to you.");
        }
    }
}
