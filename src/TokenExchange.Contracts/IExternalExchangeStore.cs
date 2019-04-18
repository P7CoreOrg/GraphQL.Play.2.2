using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts
{
    public interface IExternalExchangeStore
    {
        Task<List<ExternalExchangeRecord>> GetExternalExchangeRecordAsync();
    }
}
