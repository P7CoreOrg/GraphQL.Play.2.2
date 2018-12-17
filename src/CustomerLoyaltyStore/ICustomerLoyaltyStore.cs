using System.Text;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Commands;

namespace CustomerLoyaltyStore
{
    public interface ICustomerLoyaltyStore
    {
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> DepositEarnedLoyaltyPointsAsync(string id, int points);
        Task<Customer> DebitEarnedLoyaltyPointsAsync(string id, int points);
        Task<TransferPointsResult> TransferLoyaltyPointsAsync(string senderId, string recieverId, int points);

    }
}
