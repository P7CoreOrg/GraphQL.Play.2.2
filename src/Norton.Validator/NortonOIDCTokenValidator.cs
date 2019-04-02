using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Norton.Validator
{
    public class NortonOIDCTokenValidator : OIDCTokenValidator
    {
        public NortonOIDCTokenValidator(
            DiscoverCacheContainerFactory discoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(discoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "norton";
        }
    }
}
