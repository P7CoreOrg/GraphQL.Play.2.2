using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerLoyaltyStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using P7Core.Burner;

namespace TheApp.Pages
{
    public class IndexModel : PageModel
    {
        private ICustomerLoyaltyStore _customerLoyaltyStore;
        public Dog Dog { get; set; }
        public IndexModel(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }
        public async Task OnGetAsync()
        {
            var customer = await _customerLoyaltyStore.GetCustomerAsync("PorkyPig");
            customer = await _customerLoyaltyStore.DepositEarnedLoyaltyPointsAsync("PorkyPig", 10);
            Dog = new Dog();
        }
    }
}
