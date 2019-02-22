using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Norton.Validator
{
    public class NortonOIDCTokenValidator : OIDCTokenValidator
    {
        public NortonOIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(configuredDiscoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "norton";
        }
    }
}
