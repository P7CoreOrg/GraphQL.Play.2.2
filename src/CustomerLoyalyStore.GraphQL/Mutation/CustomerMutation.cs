using System;
using CustomerLoyaltyStore;
using CustomerLoyaltyStore.Models;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Mutation
{
    public class CustomerMutation : IMutationFieldRegistration
    {
        private ICustomerLoyaltyStore _customerLoyaltyStore;

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
                    try
                    {
                        var customer = context.GetArgument<Customer>("input");
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        customer = await _customerLoyaltyStore.DepositEarnedLoyaltyPointsAsync(customer.ID,
                            customer.LoyaltyPointBalance);
                        /*
                        var blog = context.GetArgument<SimpleDocument<Blog>>("input");

                        blog.TenantId = await _blogStore.GetTenantIdAsync();
                        await _blogStore.InsertAsync(blog);
                        */
                        return new CustomerResult()
                        {
                            ID = customer.ID,
                            LoyaltyPointBalance = customer.LoyaltyPointBalance
                        };
                       
                    }
                    catch (Exception e)
                    {

                    }
                    return false;
                    //                    return await Task.Run(() => { return ""; });

                },
                deprecationReason: null
            );
        }
    }
}