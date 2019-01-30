using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace DemoIdentityServerio.Validator
{
    public class DemoIdentityServerioOIDCTokenValidator : OIDCTokenValidator
    {
        public DemoIdentityServerioOIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(configuredDiscoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "demoidentityserverio";
        }
    }
}
