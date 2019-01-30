using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace TokenExchange.Contracts
{
    public abstract class OIDCTokenValidator: ISchemeTokenValidator
    {
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;
        private IMemoryCache _memoryCache;

        public OIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory,
            IMemoryCache memoryCache)
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
            _memoryCache = memoryCache;

        }

        public string TokenScheme { get; set; }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor.TokenScheme != TokenScheme)
            {
                throw new ArgumentException($"{nameof(tokenDescriptor.TokenScheme)} must be {TokenScheme} to use this validator");
            }
            var discoveryContainer = _configuredDiscoverCacheContainerFactory.Get(tokenDescriptor.TokenScheme);
            if (discoveryContainer == null)
            {
                throw new ArgumentException($"The OIDC AuthorityKey:{nameof(tokenDescriptor.TokenScheme)} is not supported");
            }
            var providerValidator = new ProviderValidator(discoveryContainer, _memoryCache);
            try
            {
                var principal = await providerValidator.ValidateToken(tokenDescriptor.Token,
                    new TokenValidationParameters()
                    {
                        ValidateAudience = false
                    });
                return principal;

            }
            catch (Exception e)
            {
                throw new Exception("Invalid Binding Token",e);
            }
        }
    }
}