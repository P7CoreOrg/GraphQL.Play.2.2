using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQLPlay.IdentityModelExtras;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TokenExchange.Contracts;

namespace TokenMintingService
{
    public class MicroTokenMintingService : ITokenMintingService
    {
        private IConfiguration _configuration;
        private string _authority;
        private string _clientId;
        private string _clientSecret;
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;
        private DiscoverCacheContainer _discoveryContainer;

        public MicroTokenMintingService(IConfiguration configuration,
            DiscoverCacheContainerFactory discoverCacheContainerFactory)
        {
            _configuration = configuration;
            _discoverCacheContainerFactory = discoverCacheContainerFactory;
            var scheme = _configuration["microTokenMintingService:scheme"];
            _discoveryContainer = _discoverCacheContainerFactory.Get(_configuration["microTokenMintingService:scheme"]);

            _clientId = _configuration["microTokenMintingService:clientId"];
            _clientSecret = _configuration["microTokenMintingService:clientSecret"];
        }

        Dictionary<string, string> MakeParamaters(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {
            return new Dictionary<string, string>()
            {
                {
                    OidcConstants.TokenRequest.Scope, resourceOwnerTokenRequest.Scope
                },
                {
                    "arbitrary_claims",
                    JsonConvert.SerializeObject(resourceOwnerTokenRequest.ArbitraryClaims)
                },
                {
                    "subject", resourceOwnerTokenRequest.Subject
                },
                {"access_token_lifetime", $"{resourceOwnerTokenRequest.AccessTokenLifetime}"}
            };
        }

        static TokenMintingResponse ToTokenMintingResponse(TokenResponse tokenResponse)
        {
            return new TokenMintingResponse()
            {
                AccessToken = tokenResponse.AccessToken,
                ErrorDescription = tokenResponse.ErrorDescription,
                ExpiresIn = tokenResponse.ExpiresIn,
                IdentityToken = tokenResponse.IdentityToken,
                RefreshToken = tokenResponse.RefreshToken,
                TokenType = tokenResponse.TokenType,
                IsError = tokenResponse.IsError,
                Error = tokenResponse.Error
            };
        }
        public async Task<TokenMintingResponse> MintResourceOwnerTokenAsync(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {

            Dictionary<string, string> paramaters = MakeParamaters(resourceOwnerTokenRequest);
            var client = new HttpClient();
            var discoveryResponse = await _discoveryContainer.DiscoveryCache.GetAsync();
            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                GrantType = "arbitrary_resource_owner",
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Parameters = paramaters
            });
           

            return ToTokenMintingResponse(response);

        }

        public Task<TokenMintingResponse> MintIdentityTokenAsync(IdentityTokenRequest identityTokenRequest)
        {
            throw new NotImplementedException();
        }
    }
}
