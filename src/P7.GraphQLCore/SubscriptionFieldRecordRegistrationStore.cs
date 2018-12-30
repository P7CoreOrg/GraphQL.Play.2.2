using System.Collections.Generic;
using System.Linq;

namespace P7.GraphQLCore
{
    public class SubscriptionFieldRecordRegistrationStore :
        ISubscriptionFieldRecordRegistrationStore
    {
        private List<ISubscriptionFieldRegistration> _fieldRecordRegistrations;

        public SubscriptionFieldRecordRegistrationStore(
            IEnumerable<ISubscriptionFieldRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations.ToList();
        }

        public int Count => _fieldRecordRegistrations.Count;

        public void AddGraphTypeFields(SubscriptionCore subscriptionCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(subscriptionCore);
            }
        }
    }
}