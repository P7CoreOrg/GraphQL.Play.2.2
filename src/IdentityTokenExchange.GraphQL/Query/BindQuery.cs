﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using GraphQL;
using GraphQL.Types;
using GraphQLPlay.Contracts;
using GraphQLPlay.IdentityModelExtras;
using IdentityModel;
using IdentityModel.Client;
using IdentityTokenExchangeGraphQL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using P7Core.GraphQLCore;
using TokenExchange.Contracts;

namespace IdentityTokenExchangeGraphQL.Query
{
    public class BindQuery : IQueryFieldRegistration
    {
        private ITokenValidator _tokenValidator;
        private IScopedSummaryLogger _scopedSummaryLogger;
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;
        private DiscoverCacheContainer _discoveryContainer;
        private IMemoryCache _memoryCache;
        private ProviderValidator _providerValidator;
        private ITokenMintingService _tokenMintingService;
        private IConfiguration _configuration;
        private string _scheme;
        private IPrincipalEvaluatorRouter _principalEvaluatorRouter;

        public BindQuery(
            ITokenMintingService tokenMintingService,
            IPrincipalEvaluatorRouter principalEvaluatorRouter,
            IConfiguration configuration,
            DiscoverCacheContainerFactory discoverCacheContainerFactory,
            IMemoryCache memoryCache,
            ITokenValidator tokenValidator,
            IScopedSummaryLogger scopedSummaryLogger)
        {
            _tokenMintingService = tokenMintingService;
            _principalEvaluatorRouter = principalEvaluatorRouter;
            _configuration = configuration;
            _scheme = _configuration["authValidation:scheme"];
            _discoverCacheContainerFactory = discoverCacheContainerFactory;
            _discoveryContainer = _discoverCacheContainerFactory.Get(_scheme);
            _memoryCache = memoryCache;
            _providerValidator = new ProviderValidator(_discoveryContainer, _memoryCache);
            _tokenValidator = tokenValidator;
            _scopedSummaryLogger = scopedSummaryLogger;
        }

        string GetSubjectFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                select item.Value;
            var subject = query.FirstOrDefault();
            return subject;

        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<BindResultType>(name: "bind",
                description: $"Given a proper ingres token, returns an OAuth2 payload for downstream authorized calls.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BindInput>> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        _scopedSummaryLogger.Add("query", "bind");
                        var input = context.GetArgument<BindInputModel>("input");
                        _scopedSummaryLogger.Add("tokenScheme", input.TokenScheme);

                        var requestedFields = (from item in context.SubFields
                            select item.Key).ToList();

                        var summaryLog = string.Join(";", _scopedSummaryLogger.Select(x => x.Key + "=" + x.Value).ToArray());


                        _scopedSummaryLogger.Add("requestedFields", string.Join(" ", requestedFields));

                        var principal = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                        {
                            TokenScheme = input.TokenScheme,
                            Token = input.Token
                        });
                        var subject = GetSubjectFromPincipal(principal);
                        if (string.IsNullOrEmpty(subject))
                        {
                            _scopedSummaryLogger.Add("subject", "A subject was not found in the ClaimsPrincipal object!");
                            throw new Exception("A subject was not found in the ClaimsPrincipal object!");
                        }

                        var discoveryResponse = await _discoveryContainer.DiscoveryCache.GetAsync();
                        var clientId = "arbitrary-resource-owner-client";

                        var resourceOwnerTokenRequest = await _principalEvaluatorRouter.GenerateResourceOwnerTokenRequestAsync(
                            input.Exchange,
                            principal,input.Extras);
                        resourceOwnerTokenRequest.ClientId = clientId;

                        var response =
                            await _tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);

                        if (response.IsError)
                        {
                            _scopedSummaryLogger.Add("tokenMintingServiceError", response.Error);
                            throw new Exception(response.Error);
                        }
                            

                     
                        var authorizationResultModel = new AuthorizationResultModel()
                        {
                            access_token = response.AccessToken,
                            refresh_token = response.RefreshToken,
                            expires_in = response.ExpiresIn,
                            token_type = response.TokenType,
                            authority = discoveryResponse.Issuer,
                            HttpHeaders = new List<HttpHeader>
                            {
                                new HttpHeader() {Name = "x-authScheme", Value = _scheme}
                            }

                        };
                        var bindResult = new BindResultModel
                        {
                            Authorization = authorizationResultModel
                        };
                        return bindResult;
                    }
                    catch (Exception e)
                    {
                        _scopedSummaryLogger.Add("exception", e.Message);
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);
        }
    }
}