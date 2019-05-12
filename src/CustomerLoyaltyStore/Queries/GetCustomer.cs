using System.Linq;
using CustomerLoyaltyStore.Models;
using Memstate;

namespace CustomerLoyaltyStore.Queries
{
    public class GetCustomer : Query<LoyaltyDB, Customer[]>
    {
        private readonly string _id;

        public GetCustomer(string id)
        {
            _id = id;
        }
        public override Customer[] Execute(LoyaltyDB model) => model.Customers
            .Where(c => c.Value.ID == _id)
            .Take(1).Select(c => c.Value).ToArray();
    }
}