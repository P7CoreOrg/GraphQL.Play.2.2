using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.Extensions.Caching.Memory;
using TokenExchange.Contracts;

namespace Self.Validator
{
    public class SelfOIDCTokenValidator : OIDCTokenValidator
    {
        public SelfOIDCTokenValidator(
            DiscoverCacheContainerFactory discoverCacheContainerFactory,
            IMemoryCache memoryCache) : base(discoverCacheContainerFactory, memoryCache)
        {
            TokenScheme = "self";
        }
    }
}
