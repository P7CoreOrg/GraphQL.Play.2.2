using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts.Stores
{
    public class InMemoryExternalExchangeStore : IExternalExchangeStore
    {
        public static IExternalExchangeStore MakeStore(IConfiguration configuration)
        {
            return new InMemoryExternalExchangeStore(configuration);
        }
        private IConfiguration _configuration;
        private List<ExternalExchangeRecord> _externalExchangeRecords;

        public InMemoryExternalExchangeStore(IConfiguration configuration)
        {
            _configuration = configuration;
            IConfigurationSection section = configuration.GetSection("tokenExchange:externalExchanges");
            _externalExchangeRecords = new List<ExternalExchangeRecord>();
            section.Bind(_externalExchangeRecords);
        }

        public async Task<List<ExternalExchangeRecord>> GetExternalExchangeRecordAsync()
        {
            return _externalExchangeRecords;
        }
    }
}
