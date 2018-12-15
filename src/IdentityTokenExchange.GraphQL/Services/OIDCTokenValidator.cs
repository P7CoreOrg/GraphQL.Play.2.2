using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModelExtras;
using IdentityTokenExchange.GraphQL.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace IdentityTokenExchange.GraphQL.Services
{
    public class OIDCTokenValidator: IOIDCTokenValidator
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
        public async Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor.TokenScheme != "oidc")
            {
                throw new ArgumentException($"{nameof(tokenDescriptor.TokenScheme)} must be oidc to use the OIDCTokenValidator");
            }
            var discoveryContainer = _configuredDiscoverCacheContainerFactory.Get(tokenDescriptor.AuthorityKey);
            if (discoveryContainer == null)
            {
                throw new ArgumentException($"The OIDC AuthorityKey:{nameof(tokenDescriptor.AuthorityKey)} is not supported");
            }
            var providerValidator = new ProviderValidator(discoveryContainer, _memoryCache);
            var principal = await providerValidator.ValidateToken(tokenDescriptor.Token, new TokenValidationParameters()
            {
                ValidateAudience = false
            });
            return principal;
        }
    }
}