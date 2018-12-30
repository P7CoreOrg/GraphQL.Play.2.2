namespace P7.GraphQLCore
{
    public interface ISubscriptionFieldRecordRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(SubscriptionCore subscriptionCore);
    }
}