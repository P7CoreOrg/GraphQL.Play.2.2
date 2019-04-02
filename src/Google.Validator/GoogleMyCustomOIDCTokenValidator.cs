using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Google.Validator
{
    public class GoogleMyCustomOIDCTokenValidator : OIDCTokenValidator
    {
        public GoogleMyCustomOIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory,
            IMemoryCache memoryCache) : base(configuredDiscoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "google-my-custom";
        }
    }
}