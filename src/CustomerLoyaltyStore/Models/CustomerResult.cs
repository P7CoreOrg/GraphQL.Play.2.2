using System.Collections.Generic;

namespace CustomerLoyaltyStore.Models
{
    public class CustomerResult
    {
        public CustomerResult()
        {
        }
        public string ID { get; set; }

        public int LoyaltyPointBalance { get; set; }

       public List<Prize> Prizes { get; set; }
    }
}