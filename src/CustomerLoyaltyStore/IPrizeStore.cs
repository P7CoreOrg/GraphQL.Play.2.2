using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Models;

namespace CustomerLoyaltyStore
{
    public interface IPrizeStore
    {
        Task<Prize> GetPrizeAsync(string id);
        Task<List<Prize>> GetAvailablePrizesAsync(int loyaltyPointsCost);
    }
}