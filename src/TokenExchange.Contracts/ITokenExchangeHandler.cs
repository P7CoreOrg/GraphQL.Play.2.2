using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandler
    {
        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest);
        string Name { get; }
    }
    public interface IPipelineTokenExchangeHandler
    {
        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs);
        string Name { get; }
    }
}
