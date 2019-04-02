using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Google.Validator
{
    public class GoogleOIDCTokenValidator : OIDCTokenValidator
    {
        public GoogleOIDCTokenValidator(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory,
            IMemoryCache memoryCache) : base(configuredDiscoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "google";
        }

      
    }
}
