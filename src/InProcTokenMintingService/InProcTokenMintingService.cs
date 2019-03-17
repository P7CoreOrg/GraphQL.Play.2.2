using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityModelExtras;
using IdentityServer4Extras.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TokenExchange.Contracts;

namespace TokenMintingService
{
    public class InProcTokenMintingService : ITokenMintingService
    {
        private IConfiguration _configuration;
        private string _clientId;
        private ITokenEndpointHandlerExtra _tokenEndpointHandlerExtra;
        private ILogger<InProcTokenMintingService> _logger;

        public InProcTokenMintingService(
            IConfiguration configuration,
            ITokenEndpointHandlerExtra tokenEndpointHandlerExtra,
            ILogger<InProcTokenMintingService> logger)
        {
            _configuration = configuration;
            _tokenEndpointHandlerExtra = tokenEndpointHandlerExtra;
            _logger = logger;

            _clientId = _configuration["inProcTokenMintingService:clientId"];
          
        }

       

        static TokenMintingResponse ToTokenMintingResponse(TokenRawResult tokenRawResult)
        {
            return new TokenMintingResponse()
            {
                AccessToken = tokenRawResult.TokenResult.Response.AccessToken,
                IdentityToken = tokenRawResult.TokenResult.Response.IdentityToken,
                RefreshToken = tokenRawResult.TokenResult.Response.RefreshToken,
                ExpiresIn = tokenRawResult.TokenResult.Response.AccessTokenLifetime,
                TokenType = "Bearer",
                IsError = tokenRawResult.TokenErrorResult != null,
                ErrorDescription = (tokenRawResult.TokenErrorResult == null)?"": tokenRawResult.TokenErrorResult.Response.ErrorDescription,
                Error = (tokenRawResult.TokenErrorResult == null) ? "" : tokenRawResult.TokenErrorResult.Response.Error
            };
        }

        ArbitraryResourceOwnerRequest ToArbitraryResourceOwnerRequest(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {
            var scopesList = resourceOwnerTokenRequest.Scope.Split(' ').ToList();
            var extensionGrantRequest = new ArbitraryResourceOwnerRequest()
            {
                ClientId = _clientId,
                Scopes = scopesList,
                Subject = resourceOwnerTokenRequest.Subject,
                ArbitraryClaims = resourceOwnerTokenRequest.ArbitraryClaims,
                AccessTokenLifetime = resourceOwnerTokenRequest.AccessTokenLifetime.ToString()
            };
            return extensionGrantRequest;
        }
        public async Task<TokenMintingResponse> MintResourceOwnerTokenAsync(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {
            var extensionGrantRequest = ToArbitraryResourceOwnerRequest(resourceOwnerTokenRequest);
            var result = await _tokenEndpointHandlerExtra.ProcessRawAsync(extensionGrantRequest);
            return ToTokenMintingResponse(result);
        }
    }
}
