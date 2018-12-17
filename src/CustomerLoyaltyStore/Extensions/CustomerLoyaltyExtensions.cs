using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerLoyaltyStore.Models;
using Microsoft.Extensions.Configuration;

namespace CustomerLoyaltyStore.Extensions
{
    public static class CustomerLoyaltyExtensions
    {
        public static List<Customer> LoadCustomerLoyaltyFromSettings(this IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("customerLoyalty");
            var customerRecords = new Dictionary<string, Customer>();

            section.Bind(customerRecords);
            foreach (var customerRecord in customerRecords)
            {
                var guid = new Guid(
                    System.Security.Cryptography.SHA256.Create()
                        .ComputeHash(Encoding.UTF8.GetBytes(customerRecord.Key)).Take(16).ToArray());
                customerRecord.Value.ID = customerRecord.Key;
            }

            var query = from item in customerRecords
                select item.Value;
            return query.ToList();

        }
    }
}
