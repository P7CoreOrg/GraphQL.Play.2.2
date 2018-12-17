using System.Collections.Generic;
using CustomerLoyaltyStore.Models;

namespace CustomerLoyaltyStore
{
    public class LoyaltyDB
    {
        public IDictionary<string, Customer> Customers { get; } = new Dictionary<string, Customer>();
    }
}
