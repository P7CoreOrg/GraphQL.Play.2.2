using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class GoogleMyCustomIdentityPrincipalEvaluator : IdentityPrincipalEvaluator
    {
        public GoogleMyCustomIdentityPrincipalEvaluator()
        {
            Name = "google-my-custom";
        }
        public override async Task<ResourceOwnerTokenRequest>
            GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal, List<string> extras)
        {
            if (extras == null || extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }

            // for this demo, lets assume all the extras are roles.
            var roles = extras;
            roles.Add("user");
            
            ResourceOwnerTokenRequest resourceOwnerTokenRequest = new ResourceOwnerTokenRequest()
            {
                AccessTokenLifetime = 3600,
                ArbitraryClaims = new Dictionary<string, List<string>>()
                {
                    {"role", roles}
                },
                Scope = "offline_access graphQLPlay",
                Subject = GetSubjectFromPincipal(principal)
            };
            return resourceOwnerTokenRequest;
        }
    }
}