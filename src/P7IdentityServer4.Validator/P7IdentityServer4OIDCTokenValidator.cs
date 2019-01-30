using System;
using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace P7IdentityServer4.Validator
{
    public class P7IdentityServer4OIDCTokenValidator : OIDCTokenValidator
    {
        public P7IdentityServer4OIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(configuredDiscoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "p7identityserver4";
        }
    }
}
