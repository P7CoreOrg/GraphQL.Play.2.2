using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts.Stores
{
    public class InMemoryPipelineExchangeStore : IPipelineExchangeStore
    {
        public static IPipelineExchangeStore MakeStore(IConfiguration configuration)
        {
            return new InMemoryPipelineExchangeStore(configuration);
        }
        private IConfiguration _configuration;
        private List<PipelineExchangeRecord> _pipelineExchangeRecords;

        public InMemoryPipelineExchangeStore(IConfiguration configuration)
        {
            _configuration = configuration;
            IConfigurationSection section = configuration.GetSection("tokenExchange:pipelineExchanges");
            _pipelineExchangeRecords = new List<PipelineExchangeRecord>();
            section.Bind(_pipelineExchangeRecords);
        }
        public async Task<List<PipelineExchangeRecord>> GetPipelineExchangeRecordAsync()
        {
            return _pipelineExchangeRecords;
        }
    }
}
