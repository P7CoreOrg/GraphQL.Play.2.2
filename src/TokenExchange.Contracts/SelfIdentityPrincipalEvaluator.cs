using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class SelfIdentityPrincipalEvaluator : IdentityPrincipalEvaluator
    {
        public SelfIdentityPrincipalEvaluator()
        {
            Name = "self";
        }

        public override async Task<ResourceOwnerTokenRequest>
            GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal, List<string> extras)
        {
            return await base.GenerateResourceOwnerTokenRequestAsync(principal, extras);
        }
    }

  
}