using Memstate;

namespace CustomerLoyaltyStore.Commands
{
    public class UpsertCustomer : Command<LoyaltyDB, Customer>
    {
        public UpsertCustomer(string customerId, int points)
        {
            CustomerId = customerId;
            Points = points;
        }

        public string CustomerId { get; }

        public int Points { get; }

        // it is safe to return a live customer object linked to the Model because
        // (1) the class is serializable and a remote client will get a serialized copy
        // and (2) in this particular case Customer is immutable.
        // if you have mutable classes, then please rather return a view, e.g. CustomerBalance or CustomerView class 
        public override Customer Execute(LoyaltyDB model)
        {
            Customer customer;
            if (model.Customers.TryGetValue(CustomerId, out customer))
            {
                customer.LoyaltyPointBalance += Points;
                return customer;
            }
            customer = new Customer(CustomerId, Points);
            model.Customers[CustomerId] = customer;
            return customer;
        }
    }
}