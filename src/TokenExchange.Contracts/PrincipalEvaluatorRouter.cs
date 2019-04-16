using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class PrincipalEvaluatorRouter : IPrincipalEvaluatorRouter
    {
        private IEnumerable<IPrincipalEvaluator> _principalEvaluators;

        readonly Dictionary<string, IPrincipalEvaluator> _mapPrincipalEvaluators;
        public PrincipalEvaluatorRouter(IEnumerable<IPrincipalEvaluator> principalEvaluators)
        {
            _mapPrincipalEvaluators = new Dictionary<string, IPrincipalEvaluator>();
            _principalEvaluators = principalEvaluators;
            foreach (var principalEvaluator in _principalEvaluators)
            {
                _mapPrincipalEvaluators.Add(principalEvaluator.Name, principalEvaluator);
            }
        }
        string GetSubjectFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                select item.Value;
            var subject = query.FirstOrDefault();
            return subject;

        }

        public async Task<ResourceOwnerTokenRequest> GenerateResourceOwnerTokenRequestAsync(string tokenScheme, ClaimsPrincipal principal, List<string> extras)

        {
            if (_mapPrincipalEvaluators.ContainsKey(tokenScheme))
            {
                var resourceOwnerTokenRequest =
                    await _mapPrincipalEvaluators[tokenScheme]
                        .GenerateResourceOwnerTokenRequestAsync(principal,extras);
                return resourceOwnerTokenRequest;
            }
            throw new Exception($"{tokenScheme} is not mapped to an IPrincipalEvaluator");
        }

        public async Task<TokenExchangeResponse> ProcessExchangeAsync(string tokenScheme, TokenExchangeRequest tokenExchangeRequest)
        {
            if (_mapPrincipalEvaluators.ContainsKey(tokenScheme))
            {
                var response =
                    await _mapPrincipalEvaluators[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest);
                return response;
            }
            throw new Exception($"{tokenScheme} is not mapped to an IPrincipalEvaluator");
        }
    }
}