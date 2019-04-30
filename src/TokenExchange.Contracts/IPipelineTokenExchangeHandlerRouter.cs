using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPipelineTokenExchangeHandlerRouter
    {
        Task<bool> PipelineTokenExchangeHandlerExistsAsync(string tokenScheme);

        Task<List<TokenExchangeResponse>> ProcessFinalPipelineExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs);
    }
}