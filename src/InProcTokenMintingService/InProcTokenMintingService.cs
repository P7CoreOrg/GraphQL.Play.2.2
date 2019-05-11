using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityModelExtras;
using IdentityServer4Extras.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P7Core.Utils;
using TokenExchange.Contracts;
[assembly: InternalsVisibleTo("UnitTestProject_TokenExchange")]
[assembly: InternalsVisibleTo("XUnitServer_TokenExchange")]

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


        internal static TokenMintingResponse ToTokenMintingResponse(TokenRawResult tokenRawResult)
        {
            return new TokenMintingResponse()
            {
                AccessToken = tokenRawResult.TokenResult.Response.AccessToken,
                IdentityToken = tokenRawResult.TokenResult.Response.IdentityToken,
                RefreshToken = tokenRawResult.TokenResult.Response.RefreshToken,
                ExpiresIn = tokenRawResult.TokenResult.Response.AccessTokenLifetime,
                TokenType = "Bearer",
                IsError = tokenRawResult.TokenErrorResult != null,
                ErrorDescription = (tokenRawResult.TokenErrorResult == null) ? "" : tokenRawResult.TokenErrorResult.Response.ErrorDescription,
                Error = (tokenRawResult.TokenErrorResult == null) ? "" : tokenRawResult.TokenErrorResult.Response.Error,
                Scheme = "self"
            };
        }

        internal ArbitraryResourceOwnerRequest ToArbitraryResourceOwnerRequest(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {
            Guard.ArgumentNotNull(nameof(resourceOwnerTokenRequest), resourceOwnerTokenRequest);
            var valid = !string.IsNullOrWhiteSpace(_clientId) ||
                        !string.IsNullOrWhiteSpace(resourceOwnerTokenRequest.ClientId);
            Guard.OperationValid(valid, "all clientId(s) are null!");

            var scopesList = resourceOwnerTokenRequest.Scope.Split(' ').ToList();
            var extensionGrantRequest = new ArbitraryResourceOwnerRequest()
            {
                ClientId = string.IsNullOrEmpty(resourceOwnerTokenRequest.ClientId) ? _clientId : resourceOwnerTokenRequest.ClientId,
                Scopes = scopesList,
                Subject = resourceOwnerTokenRequest.Subject,
                ArbitraryClaims = resourceOwnerTokenRequest.ArbitraryClaims,
                AccessTokenLifetime = resourceOwnerTokenRequest.AccessTokenLifetime.ToString()
            };
            return extensionGrantRequest;
        }

        internal ArbitraryIdentityRequest ToArbitraryIdentityRequest(IdentityTokenRequest identityTokenRequest)
        {
            Guard.ArgumentNotNull(nameof(identityTokenRequest), identityTokenRequest);
            var valid = !string.IsNullOrWhiteSpace(_clientId) ||
                        !string.IsNullOrWhiteSpace(identityTokenRequest.ClientId);
            Guard.OperationValid(valid, "all clientId(s) are null!");
            var scopesList = identityTokenRequest.Scope.Split(' ').ToList();
            var extensionGrantRequest = new ArbitraryIdentityRequest()
            {
                ClientId = string.IsNullOrEmpty(identityTokenRequest.ClientId) ? _clientId : identityTokenRequest.ClientId,
                Scopes = scopesList,
                Subject = identityTokenRequest.Subject,
                ArbitraryClaims = identityTokenRequest.ArbitraryClaims,
                AccessTokenLifetime = identityTokenRequest.AccessTokenLifetime?.ToString()
            };
            return extensionGrantRequest;
        }

        public async Task<TokenMintingResponse> MintResourceOwnerTokenAsync(ResourceOwnerTokenRequest resourceOwnerTokenRequest)
        {
            var extensionGrantRequest = ToArbitraryResourceOwnerRequest(resourceOwnerTokenRequest);
            var result = await _tokenEndpointHandlerExtra.ProcessRawAsync(extensionGrantRequest);
            return ToTokenMintingResponse(result);
        }

        public async Task<TokenMintingResponse> MintIdentityTokenAsync(IdentityTokenRequest identityTokenRequest)
        {
            var extensionGrantRequest = ToArbitraryIdentityRequest(identityTokenRequest);
            var result = await _tokenEndpointHandlerExtra.ProcessRawAsync(extensionGrantRequest);
            return ToTokenMintingResponse(result);
        }
    }
}
