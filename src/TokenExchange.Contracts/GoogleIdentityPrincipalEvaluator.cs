using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class GoogleIdentityPrincipalEvaluator : IdentityPrincipalEvaluator
    {
        public GoogleIdentityPrincipalEvaluator()
        {
            Name = "google";
        }
        public override async Task<ResourceOwnerTokenRequest>
            GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal, List<string> extras)
        {
            return await base.GenerateResourceOwnerTokenRequestAsync(principal, extras);
        }
    }
}