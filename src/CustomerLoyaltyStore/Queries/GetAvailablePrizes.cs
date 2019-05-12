using System.Linq;
using CustomerLoyaltyStore.Models;
using Memstate;

namespace CustomerLoyaltyStore.Queries
{
    public class GetAvailablePrizes : Query<LoyaltyDB, Prize[]>
    {
        private int _loyaltyPointsCost;

        public GetAvailablePrizes(int loyaltyPointsCost)
        {
            _loyaltyPointsCost = loyaltyPointsCost;
        }
        /*
        public override Prize[] Execute(LoyaltyDB db) => db.Prizes
            .Where(c => c.Value.LoyaltyPointsCost <= _loyaltyPointsCost)
            .Select(c => c.Value).ToArray();
            */
        public override Prize[] Execute(LoyaltyDB model)
        {
            var query = from item in model.Prizes
                        where item.Value.LoyaltyPointsCost <= _loyaltyPointsCost
                        select item.Value;
            return query.ToArray();
        }
    }
}