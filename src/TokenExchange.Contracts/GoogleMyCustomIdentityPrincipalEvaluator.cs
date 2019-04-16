using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenExchange.Contracts.Extensions;

namespace TokenExchange.Contracts
{
    public class GoogleMyCustomIdentityPrincipalEvaluator : IPrincipalEvaluator
    {

        private IHttpContextAccessor _httpContextAssessor;
        private ITokenMintingService _tokenMintingService;

        public GoogleMyCustomIdentityPrincipalEvaluator(IHttpContextAccessor httpContextAssessor, ITokenMintingService tokenMintingService)
        {
            _httpContextAssessor = httpContextAssessor;
            _tokenMintingService = tokenMintingService;
        }

        public string Name => "google-my-custom";
  
        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            if (tokenExchangeRequest.Extras == null || tokenExchangeRequest.Extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }

            // for this demo, lets assume all the extras are roles.
            var roles = tokenExchangeRequest.Extras;
            roles.Add("user");

            ResourceOwnerTokenRequest resourceOwnerTokenRequest = new ResourceOwnerTokenRequest()
            {
                AccessTokenLifetime = 3600,
                ArbitraryClaims = new Dictionary<string, List<string>>()
                {
                    { "role", roles }
                },
                Scope = "offline_access graphQLPlay",
                Subject = tokenExchangeRequest.ValidatedTokens[0].Principal.GetSubjectFromPincipal(),
                ClientId = "arbitrary-resource-owner-client"
            };
            var response = await _tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            var tokenExchangeResponse = new TokenExchangeResponse()
            {
                access_token = response.AccessToken,
                refresh_token = response.RefreshToken,
                expires_in = response.ExpiresIn,
                token_type = response.TokenType,
                authority = $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                HttpHeaders = new List<HttpHeader>
                            {
                                new HttpHeader() {Name = "x-authScheme", Value = response.Scheme}
                            }

            };
            return new List<TokenExchangeResponse>() { tokenExchangeResponse };
        }
    }
}