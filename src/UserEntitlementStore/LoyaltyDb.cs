using System.Collections.Generic;

namespace CustomerLoyaltyStore
{
    public class LoyaltyDB
    {
        public IDictionary<int, Customer> Customers { get; } = new Dictionary<int, Customer>();
    }
}
