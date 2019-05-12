using System;
using CustomerLoyaltyStore;
using CustomerLoyaltyStore.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Mutation
{
    public class CustomerMutation : IMutationFieldRegistration
    {
        private readonly ICustomerLoyaltyStore _customerLoyaltyStore;

        public CustomerMutation(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<CustomerResultType>(name: "customer",
                description: null,
                arguments: new QueryArguments(new QueryArgument<CustomerMutationInput> { Name = "input" }),
                resolve: async context =>
                {
                    var customer = context.GetArgument<Customer>("input");
                    var userContext = context.UserContext.As<GraphQLUserContext>();
                    customer = await _customerLoyaltyStore.DepositEarnedLoyaltyPointsAsync(customer.ID,
                        customer.LoyaltyPointBalance);
                    return new CustomerResult()
                    {
                        ID = customer.ID,
                        LoyaltyPointBalance = customer.LoyaltyPointBalance
                    };
                },
                deprecationReason: null
            );
        }
    }
}