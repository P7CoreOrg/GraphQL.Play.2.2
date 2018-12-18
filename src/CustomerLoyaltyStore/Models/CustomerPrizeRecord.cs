namespace CustomerLoyaltyStore.Models
{
    public class CustomerPrizeRecord
    {
        public Customer Customer { get; private set; }
        public Prize Prize { get; private set; }
        public CustomerPrizeRecord() { }

        public CustomerPrizeRecord(Customer customer, Prize prize)
        {
            Customer = customer;
            Prize = prize;
        }

    }
}