namespace CustomerLoyaltyStore.Models
{
    public class LoyaltyPointsTransfer
    {
        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public int Points { get; set; }
        public LoyaltyPointsTransfer()
        {
        }
        public LoyaltyPointsTransfer(string senderId, string receiverId, int points)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Points = points;
        }
       

        public override string ToString() => $"SenderCustomer[{SenderId}] => ReceiverCustomer[{ReceiverId}]: {Points} points.";
    }
}