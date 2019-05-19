using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
using Microsoft.AspNetCore.Http;
using TokenExchange.Contracts;
using TokenExchange.Contracts.Extensions;
using Utils.Models;

namespace BriarRabbitTokenExchange.Services
{
    public class BriarRabbitInProcTokenExchangeHandler : ITokenExchangeHandler
    {
        private IHttpContextAccessor _httpContextAssessor;
        private ISummaryLogger _summaryLogger;
        private ITokenValidator _tokenValidator;
        private ITokenMintingService _tokenMintingService;

        public BriarRabbitInProcTokenExchangeHandler(
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

        public string Name => "briar-rabbit-inproc";

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            if (tokenExchangeRequest.Extras == null || tokenExchangeRequest.Extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }
            List<ValidatedToken> validatedIdentityTokens = new List<ValidatedToken>();
            foreach (var item in tokenExchangeRequest.Tokens)
            {
                var principal = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                {
                    TokenScheme = item.TokenScheme,
                    Token = item.Token
                });
                var sub = principal.GetSubjectFromPincipal();
                if (string.IsNullOrEmpty(sub))
                {
                    _summaryLogger.Add("subject", "A subject was not found in the ClaimsPrincipal object!");
                    throw new Exception("A subject was not found in the ClaimsPrincipal object!");
                }
                validatedIdentityTokens.Add(new ValidatedToken
                {
                    Token = item.Token,
                    TokenScheme = item.TokenScheme,
                    Principal = principal
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

            var resourceOwnerResponse = await _tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);

            if (resourceOwnerResponse.IsError)
            {
                throw new Exception(resourceOwnerResponse.Error);
            }

            var tokenExchangeResponse = new TokenExchangeResponse()
            {
                accessToken = new AccessTokenResponse()
                {
                    hint = nameof(BriarRabbitInProcTokenExchangeHandler),
                    access_token = resourceOwnerResponse.AccessToken,
                    refresh_token = resourceOwnerResponse.RefreshToken,
                    expires_in = resourceOwnerResponse.ExpiresIn,
                    token_type = resourceOwnerResponse.TokenType,
                    authority =
                        $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                    HttpHeaders = new List<HttpHeader>
                    {
                        new HttpHeader() {Name = "x-authScheme", Value = resourceOwnerResponse.Scheme}
                    }

                }
            };


            IdentityTokenRequest tokenRequest = new IdentityTokenRequest()
            {

                IdentityTokenLifetime = 3600,
                ArbitraryClaims = new Dictionary<string, List<string>>()
                {
                    { "role", roles }
                },
                Scope = "graphQLPlay",
                Subject = validatedIdentityTokens[0].Principal.GetSubjectFromPincipal(),
                ClientId = "arbitrary-resource-owner-client"
            };

            var identityResponse =
                await _tokenMintingService.MintIdentityTokenAsync(tokenRequest);
            if (identityResponse.IsError)
            {
                throw new Exception(identityResponse.Error);
            }
            var identityTokenExchangeResponse = new TokenExchangeResponse()
            {
                IdentityToken = new IdentityTokenResponse()
                {
                    hint = nameof(BriarRabbitInProcTokenExchangeHandler),
                    id_token = identityResponse.IdentityToken,
                    expires_in = identityResponse.ExpiresIn,
                    authority =
                        $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                    HttpHeaders = new List<HttpHeader>
                    {
                        new HttpHeader() {Name = "x-authScheme", Value = identityResponse.Scheme}
                    }
                }
            };

            return new List<TokenExchangeResponse>()
            {
                new TokenExchangeResponse()
                {
                    customToken =  new CustomTokenResponse()
                    {
                        authority = Guid.NewGuid().ToString(),
                        hint = "briar_rabbit/token-exchange-validator/custom",
                        Type = Guid.NewGuid().ToString(),
                        Token = Guid.NewGuid().ToString(),
                        HttpHeaders = new List<HttpHeader>()
                        {
                            new HttpHeader()
                            {
                                Name = Guid.NewGuid().ToString(),
                                Value = Guid.NewGuid().ToString()
                            }
                        }
                    },
                },
                identityTokenExchangeResponse,
                tokenExchangeResponse
            };
        }
    }
}