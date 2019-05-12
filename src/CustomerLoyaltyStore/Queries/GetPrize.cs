using System.Linq;
using CustomerLoyaltyStore.Models;
using Memstate;

namespace CustomerLoyaltyStore.Queries
{
    public class GetPrize : Query<LoyaltyDB, Prize[]>
    {
        private string _id;

        public GetPrize(string id)
        {
            _id = id;
        }

        public override Prize[] Execute(LoyaltyDB model) => model.Prizes
            .Where(c => c.Value.ID == _id)
            .Take(1).Select(c => c.Value).ToArray();
    }
}