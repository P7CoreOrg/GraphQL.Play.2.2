using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Commands;
using CustomerLoyaltyStore.Extensions;
using CustomerLoyaltyStore.Queries;
using Memstate;
using Microsoft.Extensions.Configuration;

namespace CustomerLoyaltyStore
{
    public interface ICustomerLoyaltyStore
    {
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> DepositEarnedLoyaltyPointsAsync(string id, int points);
        Task<Customer> DebitEarnedLoyaltyPointsAsync(string id, int points);
        Task<Customer> TransferLoyaltyPointsAsync(string idSource, string idTarget,int points);

    }

    public class CustomerLoyaltyStore : ICustomerLoyaltyStore
    {
        private IConfiguration _configuration;

        public CustomerLoyaltyStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private List<Customer> _customers;

        private List<Customer> Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = _configuration.LoadCustomerLoyaltyFromSettings();
                }

                return _customers;
            }
        }

        private Engine<LoyaltyDB> _customerLoyaltyDBEngine;

        private async Task<Engine<LoyaltyDB>> GetCustomerLoyaltyDBEngineAsync()
        {
            if (_customerLoyaltyDBEngine == null)
            {
               
                _customerLoyaltyDBEngine = await Engine.Start<LoyaltyDB>();
                foreach (var customer in Customers)
                {
                    await _customerLoyaltyDBEngine.Execute(new InitCustomer(customer.ID, customer.LoyaltyPointBalance));
                }
            }
            return _customerLoyaltyDBEngine;
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var customers = await engine.Execute(new GetCustomer(id));
            return customers.FirstOrDefault();
        }

        public Task<Customer> DepositEarnedLoyaltyPointsAsync(string id, int points)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> DebitEarnedLoyaltyPointsAsync(string id, int points)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> TransferLoyaltyPointsAsync(string idSource, string idTarget, int points)
        {
            throw new NotImplementedException();
        }
    }



}
