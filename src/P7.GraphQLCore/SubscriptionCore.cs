using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class SubscriptionCore : ObjectGraphType<object>
    {
        public SubscriptionCore(ISubscriptionFieldRecordRegistrationStore fieldStore)
        {
            Name = "subscription";
            fieldStore.AddGraphTypeFields(this);
        }
    }
}