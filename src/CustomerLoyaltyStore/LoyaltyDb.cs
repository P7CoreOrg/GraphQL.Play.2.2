using System.Collections.Generic;
using CustomerLoyaltyStore.Models;

namespace CustomerLoyaltyStore
{
    public class LoyaltyDB
    {
        public IDictionary<string, Customer> Customers { get; } = new Dictionary<string, Customer>();
        public IList<CustomerPrizeRecord> CustomerPrizeRecords { get; } = new List<CustomerPrizeRecord>();
        public IDictionary<string, Prize> Prizes { get; } = new Dictionary<string, Prize>();
    }
}
