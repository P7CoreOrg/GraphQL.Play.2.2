using Microsoft.AspNetCore.Mvc;
using OIDCPipeline.Core.AuthorizationEndpoint;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCPipeline.Core
{
    public class OIDCResponseGenerator : IOIDCResponseGenerator
    {
        private IOIDCPipelineStore _oidcPipelineStore;

        public OIDCResponseGenerator(IOIDCPipelineStore oidcPipelineStore)
        {
            _oidcPipelineStore = oidcPipelineStore;
        }
        public async Task<IActionResult> CreateIdTokenActionResultResponseAsync(
            string key, 
            NameValueCollection extras = null, 
            bool delete = true)
        {
       
            var original = await _oidcPipelineStore.GetOriginalIdTokenRequestAsync(key);
            var downstream = await _oidcPipelineStore.GetDownstreamIdTokenResponse(key);

            var header = new JwtHeader();
            var handler = new JwtSecurityTokenHandler();
            var idToken = handler.ReadJwtToken(downstream.id_token);
            var claims = idToken.Claims.ToList();
            var scope = (from item in claims where item.Type == "scope" select item).FirstOrDefault();



            var authResponse = new AuthorizeResponse()
            {
                Request = new ValidatedAuthorizeRequest
                {
                    State = original.state,
                    RedirectUri = original.redirect_uri,
                    ResponseMode = original.response_mode
                },
                IdentityToken = downstream.id_token,
                AccessToken = downstream.access_token,
                Scope = scope?.Value
            };
            var authorizeResult = new AuthorizeResult(authResponse,extras);

            if (delete)
            {
                await _oidcPipelineStore.DeleteStoredCacheAsync(key);
            }
            return authorizeResult;
        }

        
    }
}
