using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using GraphQL;
using GraphQL.Types;
using GraphQLPlay.Contracts;
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
    public class TokenExchangeQuery : IQueryFieldRegistration
    {
        private ISummaryLogger _summaryLogger;
        private IPrincipalEvaluatorRouter _principalEvaluatorRouter;

        public TokenExchangeQuery(
            IPrincipalEvaluatorRouter principalEvaluatorRouter,
            ISummaryLogger summaryLogger)
        {
            _principalEvaluatorRouter = principalEvaluatorRouter;
            _summaryLogger = summaryLogger;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<ListGraphType<TokenExchangeResponseType>>(name: "tokenExchange",
                description: $"Given a proper list of OAuth2 Tokens, returns an authorization payload for downstream authorized calls.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TokenExchangeInput>> { Name = "input" }),
                resolve: async context =>
                {
                    try
                    {
                        var graphQLUserContext = context.UserContext as GraphQLUserContext;


                        _summaryLogger.Add("query", "bind");
                        var input = context.GetArgument<BindInputModel>("input");

                        if (input.Tokens == null || input.Tokens.Count == 0)
                        {
                            throw new Exception("no tokens present in the request!");
                        }
                        var requestedFields = (from item in context.SubFields
                                               select item.Key).ToList();

                        var summaryLog = string.Join(";", _summaryLogger.Select(x => x.Key + "=" + x.Value).ToArray());


                        _summaryLogger.Add("requestedFields", string.Join(" ", requestedFields));

                        var tokens = (from item in input.Tokens
                                      let c = new TokenWithScheme()
                                      {
                                          Token = item.Token,
                                          TokenScheme = item.TokenScheme
                                      }
                                      select c).ToList();

                        var tokenExchangeRequest = new TokenExchangeRequest()
                        {
                            Tokens = tokens,
                            Extras = input.Extras
                        };
                        var tokenExchangeResponse = await _principalEvaluatorRouter.ProcessExchangeAsync(input.Exchange, tokenExchangeRequest);

                        return tokenExchangeResponse;
                    }
                    catch (Exception e)
                    {
                        _summaryLogger.Add("exception", e.Message);
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);
        }
    }
}