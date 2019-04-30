using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandlerRouter
    {
        Task<bool> TokenExchangeHandleExistsAsync(string tokenScheme);
        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(string tokenScheme, TokenExchangeRequest tokenExchangeRequest);
    }
    public interface IPipelineTokenExchangeHandlerRouter
    {
        Task<bool> PipelineTokenExchangeHandlerExistsAsync(string tokenScheme);
        Task<List<TokenExchangeResponse>> ProcessFinalPipelineExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs);
    }
}