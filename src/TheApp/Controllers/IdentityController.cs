using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityModelExtras;
using IdentityTokenExchange.GraphQL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace TheApp.Controllers
{
    public class ClaimHandle
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [DataContract]
    public class BindModel
    {
        [DataMember(Name = "id_token")]
        public string IdToken { get; set; }
    }

    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;
        private IMemoryCache _memoryCache;
        private ProviderValidator _providerValidator;
        private ConfiguredDiscoverCacheContainer _discoveryContainer;
        public IdentityController(ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory,
            IMemoryCache memoryCache)
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
            _discoveryContainer = _configuredDiscoverCacheContainerFactory.Get("p7identityserver4");
            _memoryCache = memoryCache;
            _providerValidator = new ProviderValidator(_discoveryContainer, _memoryCache);

        }

        [Route("closed")]
        [Authorize("Daffy Duck")]
        public async Task<ActionResult<IEnumerable<ClaimHandle>>> GetClosedAsync()
        {
            try
            {
                if (Request.HttpContext.User != null)
                {
                    var query = from item in Request.HttpContext.User.Claims
                        let c = new ClaimHandle() { Name = item.Type, Value = item.Value }
                        select c;
                    return new ActionResult<IEnumerable<ClaimHandle>>(query);
                }
                return NotFound();

            }
            catch (Exception e)
            {

            }
            return NotFound();
        }

      

        // GET api/values
        [HttpGet]
        [Route("open")]
        public async Task<ActionResult<IEnumerable<ClaimHandle>>> GetOpenAsync()
        {
            try
            {
                if (Request.HttpContext.User != null)
                {
                    var query = from item in Request.HttpContext.User.Claims
                        let c = new ClaimHandle() { Name = item.Type, Value = item.Value }
                        select c;
                    return new ActionResult<IEnumerable<ClaimHandle>>(query);
                }
                return NotFound();

            }
            catch (Exception e)
            {
              
            }
            return NotFound();
        } // GET api/values
        string GetSubjectFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                        where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                        select item.Value;
            var subject = query.FirstOrDefault();
            return subject;

        }
        [HttpPost]
        [Route("bind")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<object> PostBindAsync([FromForm] IFormCollection formCollection)
        {
            try
            {
                var idToken = formCollection["id_token"];
                var principal = await _providerValidator.ValidateToken(idToken, new TokenValidationParameters()
                {
                    ValidateAudience = false
                });
                var subject = GetSubjectFromPincipal(principal);
         
                var discoveryResponse = await _discoveryContainer.DiscoveryCache.GetAsync();
                var clientId = "arbitrary-resource-owner-client";

                Dictionary<string, string> paramaters = new Dictionary<string, string>()
                {
                    {
                        OidcConstants.TokenRequest.Scope,"offline_access wizard"
                    },
                    {
                       "arbitrary_claims",
                        "{'role': ['application', 'limited']}"
                    },
                    {
                        "subject",subject
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
                
            }
            return NotFound();
        }
    }
}


 
