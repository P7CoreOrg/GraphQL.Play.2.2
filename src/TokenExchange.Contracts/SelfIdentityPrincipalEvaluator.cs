using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts.Extensions;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts
{
    public class SelfIdentityPrincipalEvaluator : IPrincipalEvaluator
    {

        private IHttpContextAccessor _httpContextAssessor;
        private IServiceProvider _serviceProvider;

        public SelfIdentityPrincipalEvaluator(
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAssessor)
        {
            _serviceProvider = serviceProvider;
            _httpContextAssessor = httpContextAssessor;
        }

        public string Name => "self";

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
            var tokenMintingService = _serviceProvider.GetRequiredService<ITokenMintingService>();
            var response = await tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);

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
            return new List<TokenExchangeResponse> { tokenExchangeResponse };
        }
    }
    

  
}