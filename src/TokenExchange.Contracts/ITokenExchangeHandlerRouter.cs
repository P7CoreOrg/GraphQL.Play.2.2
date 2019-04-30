using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandlerRouter
    {
        Task<bool> TokenExchangeHandlerExistsAsync(string tokenScheme);

        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest);
    }
}