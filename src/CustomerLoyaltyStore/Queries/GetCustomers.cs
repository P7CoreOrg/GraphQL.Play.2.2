using System.Collections.Generic;
using CustomerLoyaltyStore.Models;
using Memstate;
 

namespace CustomerLoyaltyStore.Queries
{
    public class GetCustomers : Query<LoyaltyDB, IDictionary<string, Customer>>
    {
        public override IDictionary<string, Customer> Execute(LoyaltyDB db) => new Dictionary<string, Customer>(db.Customers);
    }
}
