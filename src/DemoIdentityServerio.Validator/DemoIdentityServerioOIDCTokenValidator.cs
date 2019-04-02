using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace DemoIdentityServerio.Validator
{
    public class DemoIdentityServerioOIDCTokenValidator : OIDCTokenValidator
    {
        public DemoIdentityServerioOIDCTokenValidator(
            DiscoverCacheContainerFactory discoverCacheContainerFactory, 
            IMemoryCache memoryCache) : base(discoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "demoidentityserverio";
        }
    }
}
