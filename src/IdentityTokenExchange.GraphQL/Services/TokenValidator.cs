using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using P7Core.Utils;

namespace TokenExchange.Contracts
{
    public class TokenValidator : ITokenValidator
    {
        private Dictionary<string, ISchemeTokenValidator> _mapValidators;


        public TokenValidator(
            IEnumerable<ISchemeTokenValidator> schemValidators)
        {
            _mapValidators = new Dictionary<string, ISchemeTokenValidator>();
            foreach (var schemValidator in schemValidators)
            {
                _mapValidators.Add(schemValidator.TokenScheme, schemValidator);
            }
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            Guard.ArgumentNotNull(nameof(tokenDescriptor), tokenDescriptor);
            Guard.ArgumentNotNull(nameof(tokenDescriptor.TokenScheme), tokenDescriptor.TokenScheme);
            Guard.ArgumentNotNull(nameof(tokenDescriptor.Token), tokenDescriptor.Token);
            Guard.ArgumentValid(_mapValidators.ContainsKey(tokenDescriptor.TokenScheme),
                nameof(tokenDescriptor.TokenScheme),
                $"{tokenDescriptor.TokenScheme} is not supported!");

            var validator = _mapValidators[tokenDescriptor.TokenScheme];
            return await validator.ValidateTokenAsync(tokenDescriptor);
        }
    }
}