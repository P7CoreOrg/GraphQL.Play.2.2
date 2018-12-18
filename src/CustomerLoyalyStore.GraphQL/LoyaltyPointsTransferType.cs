using CustomerLoyaltyStore.Models;
using GraphQL.Types;

namespace CustomerLoyalyStore.GraphQL
{
    public class LoyaltyPointsTransferType : ObjectGraphType<TransferPointsResult>
    {
        public LoyaltyPointsTransferType()
        {
            Name = "loyaltyPointsTransfer";
            Field<CustomerResultType>("Recipient", "The Receiver Customer");
            Field<CustomerResultType>("Sender", "The Sender Customer");
            Field(x => x.Points).Description("The points that were transferred.");
        }
    }
}