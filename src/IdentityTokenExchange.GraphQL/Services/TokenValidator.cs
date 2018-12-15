using System;
using System.Security.Claims;
using System.Threading.Tasks;
using P7.Core.Utils;

namespace IdentityTokenExchange.GraphQL.Services
{
    public class  TokenValidator : ITokenValidator
    {
        private IOIDCTokenValidator _oidcTokenValidator;

        public TokenValidator(
            IOIDCTokenValidator oidcTokenValidator)
        {
            _oidcTokenValidator = oidcTokenValidator;
        }
        public async Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            Guard.ArgumentNotNull(nameof(tokenDescriptor), tokenDescriptor);
            Guard.ArgumentNotNull(nameof(tokenDescriptor.TokenScheme), tokenDescriptor.TokenScheme);
            Guard.ArgumentNotNull(nameof(tokenDescriptor.Token), tokenDescriptor.Token);
            switch (tokenDescriptor.TokenScheme)
            {
                case Constants.TokenSchemes.OIDC:
                    return await _oidcTokenValidator.ValidateTokenAsync(tokenDescriptor);
                    break;
            }
            throw new ArgumentException($"{tokenDescriptor.TokenScheme} is not supported!");
           
        }
    }
}