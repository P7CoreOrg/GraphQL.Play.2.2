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
            var records = new Dictionary<string, Customer>();

            section.Bind(records);
            foreach (var record in records)
            {
                var guid = new Guid(
                    System.Security.Cryptography.SHA256.Create()
                        .ComputeHash(Encoding.UTF8.GetBytes(record.Key)).Take(16).ToArray());
                record.Value.ID = record.Key;
            }

            var query = from item in records
                        select item.Value;
            return query.ToList();

        }
        public static List<Prize> LoadLoyaltyPrizesFromSettings(this IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("loyaltyPrizes");
            var records = new Dictionary<string, Prize>();

            section.Bind(records);
            foreach (var record in records)
            {
                var guid = new Guid(
                    System.Security.Cryptography.SHA256.Create()
                        .ComputeHash(Encoding.UTF8.GetBytes(record.Key)).Take(16).ToArray());
                record.Value.ID = record.Key;
            }

            var query = from item in records
                        select item.Value;
            return query.ToList();

        }
    }
}
