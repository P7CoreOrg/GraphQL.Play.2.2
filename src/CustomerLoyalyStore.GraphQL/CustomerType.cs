using System;
using CustomerLoyaltyStore;
using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Name = "customer";

            Field(x => x.ID).Description("customer Id.");
            Field(x => x.LoyaltyPointBalance).Description("Customers loyalty points balance.");
        }
    }
}
