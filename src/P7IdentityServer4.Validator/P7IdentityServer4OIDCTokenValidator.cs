using System;
using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace P7IdentityServer4.Validator
{
    public class P7IdentityServer4OIDCTokenValidator : OIDCTokenValidator
    {
        public P7IdentityServer4OIDCTokenValidator(
            DiscoverCacheContainerFactory discoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(discoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "p7identityserver4";
        }
    }
}
