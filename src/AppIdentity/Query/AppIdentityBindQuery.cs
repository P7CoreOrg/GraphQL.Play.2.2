using AppIdentity.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TokenExchange.Contracts;

namespace AppIdentity.Query
{
    public class AppIdentityBindQuery : IQueryFieldRegistration
    {
        private ITokenMintingService _tokenMintingService;

        public AppIdentityBindQuery(ITokenMintingService tokenMintingService)
        {
            _tokenMintingService = tokenMintingService;

        }
        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<AppIdentityResultType>(name: "appIdentityBind",
                description: $"Issues an application identity.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AppIdentityBindInput>> { Name = "input" }),
                resolve: async context =>
                {
                    try
                    {
                        var input = context.GetArgument<AppIdentityBindInputModel>("input");
                        var identityRequest = new IdentityTokenRequest()
                        {
                            Subject = Guid.NewGuid().ToString(),
                            ArbitraryClaims = new Dictionary<string, List<string>>
                            {
                                { "appId", new List<string> { input.AppId } },
                                { "machineId", new List<string> { input.MachineId } }
                            },
                            Scope = "arbitrary_identity",
                            ClientId = "app-identity-client"
                        };
                        var identityResult = await _tokenMintingService.MintIdentityTokenAsync(identityRequest);
                        var jwt = new JwtSecurityTokenHandler().ReadToken(identityResult.IdentityToken) as JwtSecurityToken;

                        var bindResult = new AppIdentityResultModel
                        {
                            authority = jwt.Issuer,
                            expires_in = jwt.Payload.Exp == null ? 0 : (int)jwt.Payload.Exp,
                            id_token = identityResult.IdentityToken
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
