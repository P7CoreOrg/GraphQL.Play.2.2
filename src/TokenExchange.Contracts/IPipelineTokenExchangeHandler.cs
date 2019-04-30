using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPipelineTokenExchangeHandler
    {
        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs);
        string Name { get; }
    }
}