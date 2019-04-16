using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPrincipalEvaluatorRouter
    {
        Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(string tokenScheme, 
            ClaimsPrincipal principal, List<string> extras);
        Task<TokenExchangeResponse> ProcessExchangeAsync(string tokenScheme, TokenExchangeRequest tokenExchangeRequest);

    }
}