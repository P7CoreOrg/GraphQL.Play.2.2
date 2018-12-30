using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class SubscriptionCore : ObjectGraphType<object>
    {
        private int _count;

        public SubscriptionCore(ISubscriptionFieldRecordRegistrationStore fieldStore)
        {
            Name = "subscription";
            fieldStore.AddGraphTypeFields(this);
            _count = fieldStore.Count;
        }
        public int RegistrationCount
        {
            get
            {
                return _count;
            }
        }
    }
}