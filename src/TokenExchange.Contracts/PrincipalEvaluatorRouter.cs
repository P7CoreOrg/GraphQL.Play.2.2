using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class PrincipalEvaluatorRouter : IPrincipalEvaluatorRouter
    {
        private IEnumerable<IPrincipalEvaluator> _principalEvaluators;

        Dictionary<string, IPrincipalEvaluatorRouter> _mapPrincipalEvaluators=>new Dictionary<string, IPrincipalEvaluatorRouter>();
        public PrincipalEvaluatorRouter(IEnumerable<IPrincipalEvaluator> principalEvaluators)
        {
            _principalEvaluators = principalEvaluators;
        }
        string GetSubjectFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                select item.Value;
            var subject = query.FirstOrDefault();
            return subject;

        }

        public async Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(string tokenScheme, ClaimsPrincipal principal)
        {
            ResourceOwnerTokenRequest resourceOwnerTokenRequest = new ResourceOwnerTokenRequest()
            {
                AccessTokenLifetime = 3600,
                ArbitraryClaims = new Dictionary<string, List<string>>()
                {
                    {"role", new List<string>() {"application", "limited"}}
                },
                Scope = "offline_access graphQLPlay",
                Subject = GetSubjectFromPincipal(principal)
            };
            return resourceOwnerTokenRequest;
        }
    }
}