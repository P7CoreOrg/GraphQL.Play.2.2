using System.Collections.Generic;

namespace CustomerLoyaltyStore
{
    public class LoyaltyDB
    {
        public IDictionary<string, Customer> Customers { get; } = new Dictionary<string, Customer>();
    }
}
