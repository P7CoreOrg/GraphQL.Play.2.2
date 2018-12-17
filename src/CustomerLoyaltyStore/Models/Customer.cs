namespace CustomerLoyaltyStore.Models
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string id, int loyaltyPointBalance)
        {
            ID = id;
            LoyaltyPointBalance = loyaltyPointBalance;
        }

        public string ID { get; set; }

        public int LoyaltyPointBalance { get; set; }

        public override string ToString() => $"Customer[{ID}] balance {LoyaltyPointBalance} points.";
    }
}