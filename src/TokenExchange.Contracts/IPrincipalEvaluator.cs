using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPrincipalEvaluator
    {
        Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest);
        Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal, List<string> extras);
        string Name { get; }
    }
}
