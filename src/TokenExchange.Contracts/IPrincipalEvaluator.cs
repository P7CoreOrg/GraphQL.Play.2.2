using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPrincipalEvaluator
    {
        Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal);
        string Name { get; }
    }
}
