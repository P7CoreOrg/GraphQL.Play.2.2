using CustomerLoyaltyStore.Models;
using Memstate;

namespace CustomerLoyaltyStore.Commands
{
    public class InsertPrize : Command<LoyaltyDB, Prize>
    {
        public InsertPrize(string prizeId, int costPoints)
        {
            PrizeId = prizeId;
            CostPoints = costPoints;
        }

        public string PrizeId { get; }

        public int CostPoints { get; }

        // it is safe to return a live customer object linked to the Model because
        // (1) the class is serializable and a remote client will get a serialized copy
        // and (2) in this particular case Customer is immutable.
        // if you have mutable classes, then please rather return a view, e.g. CustomerBalance or CustomerView class 
        public override Prize Execute(LoyaltyDB model)
        {
            Prize prize;
             
            if (model.Prizes.TryGetValue(PrizeId, out prize))
            {
                return prize;
            }
            prize = new Prize(PrizeId, CostPoints);
            model.Prizes[PrizeId] = prize;
            return prize;
        }
    }
}