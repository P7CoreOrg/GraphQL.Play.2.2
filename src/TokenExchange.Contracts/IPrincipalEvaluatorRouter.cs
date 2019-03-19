using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface IPrincipalEvaluatorRouter
    {
        Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(string tokenScheme, ClaimsPrincipal principal);
       
    }
}