using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModelExtras;
using IdentityModelExtras.Contracts;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Self.Validator
{
    public class SelfOIDCTokenValidator : ISchemeTokenValidator
    {
        public SelfOIDCTokenValidator(
            ISelfValidator selfValidator)
        {
            TokenScheme = "self";
            _selfValidator = selfValidator;
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            return await _selfValidator.ValidateTokenAsync(tokenDescriptor.Token);
        }

        public string TokenScheme { get; }

        private ISelfValidator _selfValidator;
    }
}
