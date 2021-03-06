﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OIDC.ReferenceWebClient.Controllers;
using OIDC.ReferenceWebClient.Extensions;
using OIDCPipeline.Core;
using OpenIdConntectModels;

namespace OIDC.ReferenceWebClient.InMemoryIdentity
{
    public class MyOpenIdConnectProtocolValidator : OpenIdConnectProtocolValidator
    {

        public override string GenerateNonce()
        {
            var sp = Global.ServiceProvider;
            var oidcPipelineStore = sp.GetRequiredService<IOIDCPipelineStore>();
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var original = oidcPipelineStore.GetOriginalIdTokenRequestAsync(httpContextAccessor.HttpContext.Session.GetSessionId()).GetAwaiter().GetResult();

            if (original != null)
            {
              
                if (!string.IsNullOrWhiteSpace(original.nonce))
                {
                    return original.nonce;
                }
            }

            string nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + Guid.NewGuid().ToString()));
            if (RequireTimeStampInNonce)
            {
                return DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture) + "." + nonce;
            }

            return nonce;
        }
    }

    public static class InMemoryIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddAuthentication<TUser>(this IServiceCollection services, IConfiguration configuration)
            where TUser : class => services.AddAuthentication<TUser>(configuration, null);

        public static IdentityBuilder AddAuthentication<TUser>(this IServiceCollection services,
            IConfiguration configuration,
            Action<IdentityOptions> setupAction)
            where TUser : class
        {
            // Services used by identity
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });

            var section = configuration.GetSection("oauth2");
            var oAuth2SchemeRecords = new List<OAuth2SchemeRecord>();
            section.Bind(oAuth2SchemeRecords);
            foreach (var record in oAuth2SchemeRecords)
            {
                var scheme = record.Scheme;
                authenticationBuilder.P7CoreAddOpenIdConnect(scheme, scheme, options =>
                {
                    options.ProtocolValidator = new MyOpenIdConnectProtocolValidator()
                    {
                        RequireTimeStampInNonce = false,
                        RequireStateValidation = false,
                        RequireNonce = true,
                        NonceLifetime = TimeSpan.FromMinutes(15)
                    };
                    options.Authority = record.Authority;
                    options.CallbackPath = record.CallbackPath;
                    options.RequireHttpsMetadata = false;
                    if (!string.IsNullOrEmpty(record.ResponseType))
                    {
                        options.ResponseType = record.ResponseType;
                    }
                    options.GetClaimsFromUserInfoEndpoint = record.GetClaimsFromUserInfoEndpoint;
                    options.ClientId = record.ClientId;
                    options.ClientSecret = record.ClientSecret;
                    options.SaveTokens = true;
                 
                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        var session = context.HttpContext.Session;
                        var stored = context.Request.GetJsonCookie<IdTokenAuthorizationRequest>(session.GetSessionId());
                        if (stored != null)
                        {
                            context.ProtocolMessage.ClientId = stored.client_id;
                            context.ProtocolMessage.ClientSecret = stored.client_secret;
                        }
                      
                        if (record.AdditionalProtocolScopes != null && record.AdditionalProtocolScopes.Any())
                        {
                            string additionalScopes = "";
                            foreach (var item in record.AdditionalProtocolScopes)
                            {
                                additionalScopes += $" {item}";
                            }
                            context.ProtocolMessage.Scope += additionalScopes;
                        }
                        if (context.HttpContext.User.Identity.IsAuthenticated)
                        {
                            // assuming a relogin trigger, so we will make the user relogin on the IDP
                            context.ProtocolMessage.Prompt = "login";
                        }
                        context.ProtocolMessage.SetParameter("idp_code", "DT");
                        /*
                        if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        {
                            context.ProtocolMessage.AcrValues = "v1=google";
                        }
                        */
                        return Task.CompletedTask;
                    };
                    options.Events.OnRemoteFailure = context =>
                    {
                        context.Response.Redirect("/");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    };
                });
            }


            return new IdentityBuilder(typeof(TUser), services);
        }
    }
}
