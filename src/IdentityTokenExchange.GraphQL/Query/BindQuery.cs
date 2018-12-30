using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using GraphQL;
using GraphQL.Types;
using IdentityModel;
using IdentityModel.Client;
using IdentityModelExtras;
using IdentityTokenExchange.GraphQL.Models;
using IdentityTokenExchange.GraphQL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using P7.GraphQLCore;

namespace IdentityTokenExchange.GraphQL.Query
{
    public class BindQuery : IQueryFieldRegistration
    {
        private ITokenValidator _tokenValidator;
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;
        private ConfiguredDiscoverCacheContainer _discoveryContainer;
        private IMemoryCache _memoryCache;
        private ProviderValidator _providerValidator;

        public BindQuery(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory,
            IMemoryCache memoryCache,
            ITokenValidator tokenValidator)
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
            _discoveryContainer = _configuredDiscoverCacheContainerFactory.Get("p7identityserver4");
            _memoryCache = memoryCache;
            _providerValidator = new ProviderValidator(_discoveryContainer, _memoryCache);
            _tokenValidator = tokenValidator;
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
                        var input = context.GetArgument<BindInputModel>("input");

                        var principal = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                        {
                            AuthorityKey = input.AuthorityKey,
                            TokenScheme = input.TokenScheme,
                            Token = input.Token
                        });
                        var subject = GetSubjectFromPincipal(principal);
                        if (string.IsNullOrEmpty(subject))
                        {
                            throw new Exception("A subject was not found in the ClaimsPrincipal object!");
                        }

                        var discoveryResponse = await _discoveryContainer.DiscoveryCache.GetAsync();
                        var clientId = "arbitrary-resource-owner-client";



                        Dictionary<string, string> paramaters = new Dictionary<string, string>()
                        {
                            {
                                OidcConstants.TokenRequest.Scope, "offline_access graphQLPlay"
                            },
                            {
                                "arbitrary_claims",
                                "{'role': ['application', 'limited']}"
                            },
                            {
                                "subject", subject
                            },
                            {"access_token_lifetime", "3600"}
                        };


                        var client = new HttpClient();

                        var response = await client.RequestTokenAsync(new TokenRequest
                        {
                            Address = discoveryResponse.TokenEndpoint,
                            GrantType = "arbitrary_resource_owner",
                            ClientId = clientId,
                            ClientSecret = "secret",

                            Parameters = paramaters
                        });
                        if (response.IsError)
                            throw new Exception(response.Error);

                        var authorizationResultModel = new AuthorizationResultModel()
                        {
                            access_token = response.AccessToken,
                            refresh_token = response.RefreshToken,
                            expires_in = response.ExpiresIn,
                            token_type = response.TokenType,
                            authority = discoveryResponse.Issuer,
                            HttpHeaders = new List<HttpHeader>
                            {
                                new HttpHeader() {Name = "x-authScheme", Value = "One"}
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
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);
        }
    }
}