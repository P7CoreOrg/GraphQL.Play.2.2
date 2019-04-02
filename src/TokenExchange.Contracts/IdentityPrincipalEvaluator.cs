using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public abstract class IdentityPrincipalEvaluator : IPrincipalEvaluator
    {
        string GetSubjectFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                select item.Value;
            var subject = query.FirstOrDefault();
            return subject;

        }
        public virtual async Task<ResourceOwnerTokenRequest> 
            GenerateResourceOwnerTokenRequestAsync(ClaimsPrincipal principal, List<string> extras)
        {
            ResourceOwnerTokenRequest resourceOwnerTokenRequest = new ResourceOwnerTokenRequest()
            {
                AccessTokenLifetime = 3600,
                ArbitraryClaims = new Dictionary<string, List<string>>()
                {
                    {"role", new List<string>() {"user"}}
                },
                Scope = "offline_access graphQLPlay",
                Subject = GetSubjectFromPincipal(principal)
            };
            return resourceOwnerTokenRequest;
        }

       
        public string Name { get; set; }
    }
}