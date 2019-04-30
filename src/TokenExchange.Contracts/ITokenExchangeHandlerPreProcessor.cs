using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandlerPreProcessor
    {
        string Name { get; }
        Task<List<KeyValuePair<string, string>>> ProcessAsync(ref TokenExchangeRequest tokenExchangeRequest);
    }
}
