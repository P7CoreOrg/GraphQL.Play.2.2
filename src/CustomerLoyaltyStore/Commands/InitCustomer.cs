using Memstate;

namespace CustomerLoyaltyStore.Commands
{
    public class InitCustomer : Command<LoyaltyDB, Customer>
    {
        public InitCustomer(string customerId, int points)
        {
            CustomerId = customerId;
            Points = points;
        }

        public string CustomerId { get; }

        public int Points { get; }

        public override Customer Execute(LoyaltyDB model)
        {
            var customer = new Customer(CustomerId, Points);
            model.Customers[CustomerId] = customer;
            return customer;
        }
    }
}