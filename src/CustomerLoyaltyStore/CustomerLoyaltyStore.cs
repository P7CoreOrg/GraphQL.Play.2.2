using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Commands;
using CustomerLoyaltyStore.Extensions;
using CustomerLoyaltyStore.Models;
using CustomerLoyaltyStore.Queries;
using Memstate;
using Microsoft.Extensions.Configuration;

namespace CustomerLoyaltyStore
{
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
        private List<Prize> _prizes;

        private List<Prize> Prizes
        {
            get
            {
                if (_prizes == null)
                {
                    _prizes = _configuration.LoadLoyaltyPrizesFromSettings();
                }

                return _prizes;
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
                    await _customerLoyaltyDBEngine.Execute(new UpsertCustomer(customer.ID, customer.LoyaltyPointBalance));
                }
                foreach (var prize in Prizes)
                {
                    await _customerLoyaltyDBEngine.Execute(new InsertPrize(prize.ID, prize.LoyaltyPointsCost));
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

        public async Task<Customer> DepositEarnedLoyaltyPointsAsync(string id, int points)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var customer = await engine.Execute(new UpsertCustomer(id, points));
            return customer;
        }

        public async Task<Customer> DebitEarnedLoyaltyPointsAsync(string id, int points)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var customer = await engine.Execute(new DebitPoints(id, points));
            return customer;
        }

        public async Task<TransferPointsResult> TransferLoyaltyPointsAsync(string senderId, string recieverId, int points)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var result = await engine.Execute(new TransferPoints(senderId, recieverId,points));
            return result;
        }

        public async Task<Prize> GetPrizeAsync(string id)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var result = await engine.Execute(new GetPrize(id));
            return result.FirstOrDefault();

        }

        public async Task<List<Prize>> GetAvailablePrizesAsync(int loyaltyPointsCost)
        {
            var engine = await GetCustomerLoyaltyDBEngineAsync();
            var result = await engine.Execute(new GetAvailablePrizes(loyaltyPointsCost));
            return new List<Prize>(result);
        }
    }
}