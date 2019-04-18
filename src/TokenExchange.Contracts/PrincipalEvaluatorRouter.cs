using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class PrincipalEvaluatorRouter : IPrincipalEvaluatorRouter
    {
        private IEnumerable<ITokenExchangeHandler> _principalEvaluators;

        readonly Dictionary<string, ITokenExchangeHandler> _mapPrincipalEvaluators;
        public PrincipalEvaluatorRouter(IEnumerable<ITokenExchangeHandler> principalEvaluators)
        {
            _mapPrincipalEvaluators = new Dictionary<string, ITokenExchangeHandler>();
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


        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(string tokenScheme, TokenExchangeRequest tokenExchangeRequest)
        {
            if (_mapPrincipalEvaluators.ContainsKey(tokenScheme))
            {
                var response =
                    await _mapPrincipalEvaluators[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest);
                return response;
            }
            throw new Exception($"{tokenScheme} is not mapped to an ITokenExchangeHandler");
        }
    }
}