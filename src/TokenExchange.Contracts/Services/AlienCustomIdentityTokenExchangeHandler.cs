using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Utils.Models;

namespace TokenExchange.Contracts.Services
{
    public class AlienCustomIdentityTokenExchangeHandler : ITokenExchangeHandler
    {

        private IHttpContextAccessor _httpContextAssessor;
        private IServiceProvider _serviceProvider;

        public AlienCustomIdentityTokenExchangeHandler(
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAssessor)
        {
            _serviceProvider = serviceProvider;
            _httpContextAssessor = httpContextAssessor;
        }

        public string Name => "alien-exchange";

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            if (tokenExchangeRequest.Extras == null || tokenExchangeRequest.Extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }

            // for this demo, lets assume all the extras are roles.
            var response = new List<TokenExchangeResponse>();
            for (int i = 0; i < 2; i++)
            {
                var tokenExchangeResponse = new TokenExchangeResponse()
                {
                    accessToken = new AccessTokenResponse()
                    {
                        hint = "Alien Response",
                        access_token = $"Alien_access_token_{Guid.NewGuid().ToString()}",
                        refresh_token = $"Alien_refresh_token_{Guid.NewGuid().ToString()}",
                        expires_in = 1234 + i,
                        token_type = $"Alien_{Guid.NewGuid().ToString()}",
                        authority = $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}/Alien",
                        HttpHeaders = new List<HttpHeader>
                        {
                            new HttpHeader() {Name = "x-authScheme", Value = "Alien"}
                        }
                    }
                };
                response.Add(tokenExchangeResponse);
            }

            return response;
        }
    }
}