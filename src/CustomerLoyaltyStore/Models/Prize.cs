namespace CustomerLoyaltyStore.Models
{
    public class Prize
    {
        public Prize()
        {
        }

        public Prize(string id, int loyaltyPointsCost)
        {
            ID = id;
            LoyaltyPointsCost = loyaltyPointsCost;
        }

        public string ID { get; set; }

        public int LoyaltyPointsCost { get; set; }

        public override string ToString() => $"Prize[{ID}] costs {LoyaltyPointsCost} points.";
    }
}