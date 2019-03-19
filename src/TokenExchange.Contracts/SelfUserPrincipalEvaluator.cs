using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class SelfUserPrincipalEvaluator : UserPrincipalEvaluator
    {
        public SelfUserPrincipalEvaluator()
        {
            Name = "self";
        }

        public override async Task<ResourceOwnerTokenRequest>
            GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal)
        {
            return await base.GenerateResourceOwnerTokenRequestAsync(principal);
        }
    }
}