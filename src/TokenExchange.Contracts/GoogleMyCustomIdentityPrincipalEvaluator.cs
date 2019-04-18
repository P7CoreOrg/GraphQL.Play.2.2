﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TokenExchange.Contracts.Extensions;
using TokenExchange.Contracts.Models;
using Utils.Models;
using GraphQLPlay.Contracts;

namespace TokenExchange.Contracts
{
    public class GoogleMyCustomIdentityPrincipalEvaluator : IPrincipalEvaluator
    {
        private IHttpContextAccessor _httpContextAssessor;
        private ISummaryLogger _summaryLogger;
        private ITokenValidator _tokenValidator;
        private ITokenMintingService _tokenMintingService;

        public GoogleMyCustomIdentityPrincipalEvaluator(
            ITokenValidator tokenValidator,
            ITokenMintingService tokenMintingService,
            IHttpContextAccessor httpContextAssessor,
            ISummaryLogger summaryLogger)
        {
            _tokenValidator = tokenValidator;
            _tokenMintingService = tokenMintingService;
            _httpContextAssessor = httpContextAssessor;
            _summaryLogger = summaryLogger;
        }

        public string Name => "google-my-custom";

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            if (tokenExchangeRequest.Extras == null || tokenExchangeRequest.Extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }
            List<ValidatedToken> validatedIdentityTokens = new List<ValidatedToken>();
            foreach (var item in tokenExchangeRequest.Tokens)
            {
                var prince = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                {
                    TokenScheme = item.TokenScheme,
                    Token = item.Token
                });
                var sub = prince.GetSubjectFromPincipal();
                if (string.IsNullOrEmpty(sub))
                {
                    _summaryLogger.Add("subject", "A subject was not found in the ClaimsPrincipal object!");
                    throw new Exception("A subject was not found in the ClaimsPrincipal object!");
                }
                validatedIdentityTokens.Add(new ValidatedToken
                {
                    Token = item.Token,
                    TokenScheme = item.TokenScheme,
                    Principal = prince
                });
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
                Subject = validatedIdentityTokens[0].Principal.GetSubjectFromPincipal(),
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