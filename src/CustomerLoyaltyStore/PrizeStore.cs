using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Models;

namespace CustomerLoyaltyStore
{
    public class PrizeStore : IPrizeStore
    {
        private readonly ICustomerLoyaltyStore _customerLoyaltyStore;

        public PrizeStore(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }
        public async Task<Prize> GetPrizeAsync(string id)
        {
            return await _customerLoyaltyStore.GetPrizeAsync(id);
        }

        public async Task<List<Prize>> GetAvailablePrizesAsync(int loyaltyPointsCost)
        {
            return await _customerLoyaltyStore.GetAvailablePrizesAsync(loyaltyPointsCost);
        }
    }
}