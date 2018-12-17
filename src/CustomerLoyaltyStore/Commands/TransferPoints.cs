using CustomerLoyaltyStore.Models;
using Memstate;

namespace CustomerLoyaltyStore.Commands
{
    public class TransferPoints : Command<LoyaltyDB, TransferPointsResult>
    {
        public TransferPoints(string senderId, string receiverId, int points)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Points = points;
        }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public int Points { get; set; }

        // it is safe to return actual references to Customer objects, because they are immutable.
        public override TransferPointsResult Execute(LoyaltyDB model)
        {
            var sender = model.Customers[SenderId];
            var receiver = model.Customers[ReceiverId];
            var newSender = new Customer(sender.ID, sender.LoyaltyPointBalance - Points);
            var newReceiver = new Customer(receiver.ID, receiver.LoyaltyPointBalance + Points);
            model.Customers[SenderId] = newSender;
            model.Customers[ReceiverId] = newReceiver;
            return new TransferPointsResult(newSender, newReceiver, Points);
        }
    }
}