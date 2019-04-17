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
        private List<ExternalExchangeClientCredentials> _externalExchangeClientCredentials;

        public InMemoryExternalExchangeStore(IConfiguration configuration)
        {
            _configuration = configuration;
            IConfigurationSection section = configuration.GetSection("tokenExchange:externalExchanges:oAuth2_client_credentials");
            _externalExchangeClientCredentials = new List<ExternalExchangeClientCredentials>();
            section.Bind(_externalExchangeClientCredentials);
        }

        public async Task<List<ExternalExchangeClientCredentials>> GetClientCredentialExchangesAsync()
        {
            return _externalExchangeClientCredentials;
        }
    }
}
