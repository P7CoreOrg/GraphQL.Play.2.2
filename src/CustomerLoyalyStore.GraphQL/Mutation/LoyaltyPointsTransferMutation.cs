using System;
using CustomerLoyaltyStore;
using CustomerLoyaltyStore.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Mutation
{
    public class LoyaltyPointsTransferMutation : IMutationFieldRegistration
    {
        private readonly ICustomerLoyaltyStore _customerLoyaltyStore;

        public LoyaltyPointsTransferMutation(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<LoyaltyPointsTransferType>(name: "loyaltyPointsTransfer",
                description: null,
                arguments: new QueryArguments(new QueryArgument<LoyaltyPointsTransferMutationInput> { Name = "input" }),
                resolve: async context =>
                {
                    var loyaltyPointsTransfer = context.GetArgument<LoyaltyPointsTransfer>("input");
                    var userContext = context.UserContext.As<GraphQLUserContext>();
                    var result = await _customerLoyaltyStore.TransferLoyaltyPointsAsync(
                        loyaltyPointsTransfer.SenderId,
                        loyaltyPointsTransfer.ReceiverId,
                        loyaltyPointsTransfer.Points);
                    return result;

                },
                deprecationReason: null
            );
        }
    }
}