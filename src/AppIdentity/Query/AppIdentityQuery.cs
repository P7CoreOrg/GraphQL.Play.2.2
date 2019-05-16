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
using GraphQL.Validation;
using TokenExchange.Contracts;

namespace AppIdentity.Query
{
    public class AppIdentityQuery : IQueryFieldRegistration
    {
        private readonly ITokenMintingService _tokenMintingService;
        private readonly ITokenValidator _tokenValidator;

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
                    catch (ExecutionError executionError)
                    {
                        context.Errors.Add(executionError);
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
                       string subject = Guid.NewGuid().ToString();
                       var input = context.GetArgument<AppIdentityCreateInputModel>("input");
                       if (!string.IsNullOrEmpty(input.Subject))
                       {
                           GraphQLUserContext userContext = context.UserContext as GraphQLUserContext;
                           var user = userContext.HttpContextAccessor.HttpContext.User;
                           
                           if (!user.Identity.IsAuthenticated || !userContext.HttpContextAccessor.HttpContext.User.HasClaim("scope", "appIdentity"))
                           {
                               throw new ValidationError(
                                   "appIdentityCreate",
                                   "auth-required",
                                   $"You are not authorized to run this query.");
                           }
                           subject = input.Subject;


                       }
                       var identityRequest = new IdentityTokenRequest()
                       {
                           Subject = subject,
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
                   catch (ExecutionError executionError)
                   {
                       context.Errors.Add(executionError);
                   }
                   return null;
               },
               deprecationReason: null);
        
        }
    }
}
