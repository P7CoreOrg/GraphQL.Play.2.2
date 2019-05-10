using AppIdentity.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TokenExchange.Contracts;

namespace AppIdentity.Query
{
    public class AppIdentityException : Exception
    {

    }
    public class AppIdentityQuery : IQueryFieldRegistration
    {
        private ITokenMintingService _tokenMintingService;
        private ITokenValidator _tokenValidator;

        public AppIdentityQuery(ITokenMintingService tokenMintingService, ITokenValidator tokenValidator)
        {
            _tokenMintingService = tokenMintingService;
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
        (string appId, string machineId) GetRequiredClaimsFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                        where item.Type == "appId"
                        select item.Value;
            var appId = query.FirstOrDefault();
            if (string.IsNullOrEmpty(appId))
            {
                throw new ExecutionError($"Required claim: appId is not pressent");
            }


            query = from item in principal.Claims
                        where item.Type == "machineId"
                        select item.Value;
            var machineId = query.FirstOrDefault();
            if (string.IsNullOrEmpty(machineId))
            {
                throw new ExecutionError($"Required claim: machineId is not pressent");
            }


            return (appId, machineId);

        }
        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<AppIdentityResultType>(name: "appIdentityRefresh",
                description: $"Issues an application identity.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AppIdentityRefreshInput>> { Name = "input" }),
                resolve: async context =>
                {
                    try
                    {
                        var input = context.GetArgument<AppIdentityRefreshInputModel>("input");
                        var principal = await _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                        {
                            TokenScheme = "self",
                            Token = input.id_token
                        });
                        var subject = GetSubjectFromPincipal(principal);
                        if (string.IsNullOrEmpty(subject))
                        {
                            throw new ExecutionError("A subject was not found in the ClaimsPrincipal object!");
                        }
                        var requiredClaims = GetRequiredClaimsFromPincipal(principal);

                        var jwt = new JwtSecurityTokenHandler().ReadToken(input.id_token) as JwtSecurityToken;


                        var identityRequest = new IdentityTokenRequest()
                        {
                            Subject = jwt.Payload.Sub,
                            ArbitraryClaims = new Dictionary<string, List<string>>
                            {
                                {"appId", new List<string> {requiredClaims.appId}},
                                {"machineId", new List<string> {requiredClaims.machineId}}
                            },
                            Scope = "arbitrary_identity",
                            ClientId = "app-identity-client"
                        };
                        var identityResult = await _tokenMintingService.MintIdentityTokenAsync(identityRequest);

                        var expiresIn = 0;
                        if (jwt.Payload.Exp != null)
                        {
                            expiresIn = (int) jwt.Payload.Exp;
                        }

                        var bindResult = new AppIdentityResultModel
                        {
                            authority = jwt.Issuer,
                            expires_in = expiresIn,
                            id_token = identityResult.IdentityToken
                        };
                        return bindResult;
                    }
                    catch (ExecutionError executionError)
                    {
                        context.Errors.Add(executionError);
                    }
                    catch (Exception e)
                    {
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);

            queryCore.FieldAsync<AppIdentityResultType>(name: "appIdentityCreate",
               description: $"Issues an application identity.",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AppIdentityCreateInput>> { Name = "input" }),
               resolve: async context =>
               {
                   try
                   {
                       var input = context.GetArgument<AppIdentityCreateInputModel>("input");
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

                       var expiresIn = 0;
                       if (jwt.Payload.Exp != null)
                       {
                           expiresIn = (int)jwt.Payload.Exp;
                       }
                       var bindResult = new AppIdentityResultModel
                       {
                           authority = jwt.Issuer,
                           expires_in = expiresIn,
                           id_token = identityResult.IdentityToken
                       };
                       return bindResult;
                   }
                   
                   catch (Exception e)
                   {
                       
                       context.Errors.Add(new ExecutionError(e.Message));
                   }

                   return null;
               },
               deprecationReason: null);
        }
    }
}
