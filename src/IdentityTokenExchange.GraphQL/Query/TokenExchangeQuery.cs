using System;
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
using TokenExchange.Contracts.Extensions;

namespace IdentityTokenExchangeGraphQL.Query
{
    public class TokenExchangeQuery : IQueryFieldRegistration
    {
        private ITokenValidator _tokenValidator;
        private IScopedSummaryLogger _scopedSummaryLogger;
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;
        private IMemoryCache _memoryCache;
        private ITokenMintingService _tokenMintingService;
        private IConfiguration _configuration;
        private string _scheme;
        private IPrincipalEvaluatorRouter _principalEvaluatorRouter;

        public TokenExchangeQuery(
            ITokenMintingService tokenMintingService,
            IPrincipalEvaluatorRouter principalEvaluatorRouter,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            ITokenValidator tokenValidator,
            IScopedSummaryLogger scopedSummaryLogger)
        {
            _tokenMintingService = tokenMintingService;
            _principalEvaluatorRouter = principalEvaluatorRouter;
            _configuration = configuration;
            _scheme = _configuration["authValidation:scheme"];
            _memoryCache = memoryCache;
            _tokenValidator = tokenValidator;
            _scopedSummaryLogger = scopedSummaryLogger;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<TokenExchangeResponseType>(name: "tokenExchange",
                description: $"Given a proper list of OAuth2 Tokens, returns an authorization payload for downstream authorized calls.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TokenExchangeInput>> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var graphQLUserContext = context.UserContext as GraphQLUserContext;
                     

                        _scopedSummaryLogger.Add("query", "bind");
                        var input = context.GetArgument<BindInputModel>("input");
                      
                        if(input.Tokens == null || input.Tokens.Count == 0)
                        {
                            throw new Exception("no tokens present in the request!");
                        }
                        var requestedFields = (from item in context.SubFields
                            select item.Key).ToList();

                        var summaryLog = string.Join(";", _scopedSummaryLogger.Select(x => x.Key + "=" + x.Value).ToArray());


                        _scopedSummaryLogger.Add("requestedFields", string.Join(" ", requestedFields));

                        List<ValidatedToken> validatedIdentityTokens = new List<ValidatedToken>();
                        foreach (var item in input.Tokens)
                        {
                            var prince = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                            {
                                TokenScheme = item.TokenScheme,
                                Token = item.Token
                            });
                            var sub = prince.GetSubjectFromPincipal();
                            if (string.IsNullOrEmpty(sub))
                            {
                                _scopedSummaryLogger.Add("subject", "A subject was not found in the ClaimsPrincipal object!");
                                throw new Exception("A subject was not found in the ClaimsPrincipal object!");
                            }
                            validatedIdentityTokens.Add(new ValidatedToken
                            {
                                Token = item.Token,
                                TokenScheme = item.TokenScheme,
                                Principal = prince
                            });
                        }

                        var tokenExchangeRequest = new TokenExchangeRequest() {
                            ValidatedTokens = validatedIdentityTokens,
                            Extras = input.Extras
                        };
                        var tokenExchangeResponse = await _principalEvaluatorRouter.ProcessExchangeAsync(input.Exchange, tokenExchangeRequest);
                        return tokenExchangeResponse;
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