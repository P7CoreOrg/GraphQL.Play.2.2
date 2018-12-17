using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL.Mutation
{
    public class LoyaltyPointsTransferMutationInput : InputObjectGraphType
    {
        public LoyaltyPointsTransferMutationInput()
        {
            Name = "loyaltyPointsTransfersMutationInput";
            Field<NonNullGraphType<StringGraphType>>("ReceiverId");
            Field<NonNullGraphType<StringGraphType>>("SenderId");
            Field<NonNullGraphType<IntGraphType>>("Points");

        }
    }
}