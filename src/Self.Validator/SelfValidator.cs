using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TokenExchange.Contracts;

namespace Self.Validator
{
    public class SelfValidator : ISelfValidator
    {

        private IValidationKeysStore _validationKeysStore;

        public SelfValidator(IMemoryCache memoryCache, IValidationKeysStore validationKeysStore)
        {
            _validationKeysStore = validationKeysStore;
            _memoryCache = memoryCache;
            _memCacheKey = Guid.NewGuid().ToString();
        }


        private IMemoryCache _memoryCache;
        private string _memCacheKey;

        private TokenValidationParameters TokenValidationParameters
        {
            get
            {
                TokenValidationParameters tokenValidationParameters = null;
                if (!_memoryCache.TryGetValue(_memCacheKey, out tokenValidationParameters))
                {
                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTimeOffset.Now.AddDays(1));

                    var issuerSigningKeys = _validationKeysStore.GetValidationKeysAsync().GetAwaiter().GetResult();
                    var tt =
                        new TokenValidationParameters
                        {

                            IssuerSigningKeys = issuerSigningKeys,
                            RequireSignedTokens = true,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateIssuerSigningKey = true

                        };
                    _memoryCache.Set(_memCacheKey, tt, cacheEntryOptions);
                    tokenValidationParameters = tt;
                }

                return tokenValidationParameters;

            }
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            try
            {

                SecurityToken validatedToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, TokenValidationParameters, out validatedToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}