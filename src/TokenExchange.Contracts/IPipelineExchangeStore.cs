using System.Collections.Generic;
using System.Threading.Tasks;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts
{
    public interface IPipelineExchangeStore
    {
        Task<List<PipelineExchangeRecord>> GetPipelineExchangeRecordAsync();
    }
}
