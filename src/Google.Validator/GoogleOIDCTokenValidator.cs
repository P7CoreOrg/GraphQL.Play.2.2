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
            DiscoverCacheContainerFactory discoverCacheContainerFactory,
            IMemoryCache memoryCache) : base(discoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "google";
        }
    }
}
