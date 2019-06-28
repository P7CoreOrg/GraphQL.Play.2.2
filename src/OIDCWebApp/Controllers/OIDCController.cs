using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityModel;

namespace OIDC.ReferenceWebClient.Controllers
{
    public class IdTokenAuthorizationRequest
    {
        /*
         client_id=mvc
                &redirect_uri=https%3A%2F%2Fp7core.127.0.0.1.xip.io%3A44311%2Fsignin-oidc
                &response_type=id_token
                &scope=openid%20profile
                &response_mode=form_post
                &nonce=636973229335838266.ZWJhM2U4M2YtYWNiYi00YjZkLTkwMWYtNjRmMjM3MWRiYTk5OWNkNDIzMWUtZmY4OS00YWE0LTk4MGUtMTdiMjYxNmNmZjRk&state=CfDJ8KOz5LEySMhBtqpccMk4UVhA1PvGQQvpqQBUyR-97TDZvaPuNquTLJIUxKMYzF-Ov_HHCnnmcTForzd5RJ4jmLONvcZLY3XCHnrhh9Sc2oR2Lv2HACvPVBMy2oYmmPBtNIoXroQ9WePE_KtPyFw8ntRsHIYMmT5a0fLKGeJcwK3ewoiRHxjKpOr9hXZau9f7CVVqMvtWC2ngWrFsEeh8S0YtRZQFT-7XyjE9dNiyKp_Z-4iBUbbqzVnT4GmEmErZXUjmBhmVsMLz5h9y_F3usRT3lg7LxUNamnJuROnYIqmJzf0fYVJq1mcB5hcUipo2SNcILG3xkUikc84VznSGvD7V_qFjOHVPtOEX02JH9M4ymb3iZtZSE9dDr2RkwTU7StoKgM-x195bBULpwms8weJO-kx5I6UrY_lmWl0SFqYN
*/
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
        public string response_type { get; set; }
        public string nonce { get; set; }
        public string response_mode { get; set; }
    }
    [ApiController]
    [Route("")]
    public class OIDCController : ControllerBase
    {
        private ILogger<OIDCController> _logger;
        private IAuthorizeRequestValidator _authorizeRequestValidator;

        public OIDCController(ILogger<OIDCController> logger,
            IAuthorizeRequestValidator authorizeRequestValidator)
        {
            _logger = logger;
            _authorizeRequestValidator = authorizeRequestValidator;
        }
        // GET: api/OIDC
        [HttpGet]
        [Route(".well-known/openid-configuration")]
        public Dictionary<string, object> GetWellknownOpenIdConfiguration()
        {
            
            var stuff = JsonConvert.DeserializeObject<Dictionary<string,object>>(@"{
  'response_types_supported' : [
    'code'
  ],
  'request_parameter_supported' : false,
  'request_uri_parameter_supported' : false,
  'claims_parameter_supported' : false,
  'grant_types_supported' : [
    'authorization_code',
    'access_token',
    'refresh_token',
    'symc_fed_idp_authorization_code',
    'symc_fed_idp_access_token',
    'session_otp'
  ],
  'scopes_supported' : [
    'address',
    'cloudconnect',
    'device',
    'device_all',
    'email',
    'family',
    'idp_admin',
    'idsc_read',
    'idsc_write',
    'lifelock',
    'ngp_ar',
    'nks_test_read',
    'nks_test_write',
    'nms_device_locate',
    'nms_device_lock',
    'norton_aggregator',
    'offline_access',
    'open_web_session',
    'openid',
    'phone',
    'profile',
    'voice_core'
  ],
  'issuer' : 'https://login-int.norton.com/sso/oidc1/token',
  'acr_values_supported' : [
    'https://login.norton.com/sso/saml_2.0_profile/settings',
    'https://login.norton.com/sso/saml_2.0_profile/readonly_username',
    'https://login.norton.com/sso/saml_2.0_profile/password',
    'https://login.norton.com/sso/saml_2.0_profile/setmktg',
    'https://login.norton.com/sso/saml_2.0_profile/noheaderfooter',
    'https://login.norton.com/sso/saml_2.0_profile/nosignup',
    'https://login.norton.com/sso/saml_2.0_profile/high_contrast',
    'https://login.norton.com/sso/saml_2.0_profile/account_rename',
    'https://login.norton.com/sso/saml_2.0_profile/loa_phone_match',
    'http://idmanagement.gov/icam/2009/12/saml_2.0_profile/assurancelevel1',
    'http://idmanagement.gov/icam/2009/12/saml_2.0_profile/assurancelevel3',
    'https://login.norton.com/sso/saml_2.0_profile/show_service_provider_link',
    'https://login.norton.com/sso/saml_2.0_profile/noemail',
    'https://login.norton.com/sso/saml_2.0_profile/allow_shared_email_address',
    'https://login.norton.com/sso/saml_2.0_profile/guid',
    'https://login.norton.com/sso/saml_2.0_profile/continue_without_authentication',
    'https://login.norton.com/sso/saml_2.0_profile/session_otp',
    'https://login.norton.com/sso/saml_2.0_profile/two_factor_auth',
    'https://login.norton.com/sso/saml_2.0_profile/norton_client',
    'https://login.norton.com/sso/saml_2.0_profile/emailAddress',
    'https://login.norton.com/sso/saml_2.0_profile/phone_number_account',
    'https://login.norton.com/sso/saml_2.0_profile/show_create_account',
    'https://login.norton.com/sso/saml_2.0_profile/loa_ll',
    'https://login.norton.com/sso/saml_2.0_profile/show_update_profile_email',
    'https://login.norton.com/sso/saml_2.0_profile/nomktg',
    'http://idmanagement.gov/icam/2009/12/saml_2.0_profile/assurancelevel2',
    'https://login.norton.com/sso/saml_2.0_profile/key'
  ],
  'authorization_endpoint' : 'https://login-int.norton.com/sso/idp/OIDC',
  'userinfo_endpoint' : 'https://login-int.norton.com/sso/oidc1/userinfo',
  'display_values_supported' : [
    'page'
  ],
  'userinfo_signing_alg_values_supported' : [
    'RS384',
    'RS256',
    'RS512'
  ],
  'claims_supported' : [
    'country',
    'birthdate',
    'CommId',
    'gender',
    'city',
    'alt_family_name',
    'locale',
    'CustomAttributes',
    'updated_at',
    'state',
    'email',
    'zip',
    'email_verified',
    'address',
    'CommAccId',
    'product_security_update',
    'given_name',
    'middle_name',
    'IdPData',
    'session_otp',
    'alt_given_name',
    'SSN',
    'LinkChildGUIDS',
    'NortonGUID',
    'UserId',
    'verified_loa_level',
    'name',
    'loa_level',
    'phone_number',
    'PartnerUserId',
    'family_name',
    'operation',
    'VipTokenId'
  ],
  'require_request_uri_registration' : false,
  'jwks_uri' : 'https://login-int.norton.com/sso/oidc1/token/jwks',
  'subject_types_supported' : [
    'pairwise'
  ],
  'id_token_signing_alg_values_supported' : [
    'RS384',
    'RS256',
    'RS512'
  ],
  'claim_types_supported' : [
    'normal',
    'aggregated'
  ],
  'token_endpoint_auth_methods_supported' : [
    'client_secret_post',
    'client_secret_basic'
  ],
  'response_modes_supported' : [
    'query'
  ],
  'token_endpoint' : 'https://login-int.norton.com/sso/oidc1/tokens'
}");

            return stuff;
        }
        
        // GET: api/OIDC/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OIDC
        [HttpPost]
        [HttpGet]
        [Route("connect/authorize")]
        public async Task<IActionResult> PostConnectAuthorizeAsync()
        {
            /*
             https://localhost:44305/connect/authorize?
client_id=mvc
&redirect_uri=https%3A%2F%2Fp7core.127.0.0.1.xip.io%3A44311%2Fsignin-oidc
&response_type=id_token
&scope=openid%20profile
&response_mode=form_post
&nonce=636973229335838266.ZWJhM2U4M2YtYWNiYi00YjZkLTkwMWYtNjRmMjM3MWRiYTk5OWNkNDIzMWUtZmY4OS00YWE0LTk4MGUtMTdiMjYxNmNmZjRk&state=CfDJ8KOz5LEySMhBtqpccMk4UVhA1PvGQQvpqQBUyR-97TDZvaPuNquTLJIUxKMYzF-Ov_HHCnnmcTForzd5RJ4jmLONvcZLY3XCHnrhh9Sc2oR2Lv2HACvPVBMy2oYmmPBtNIoXroQ9WePE_KtPyFw8ntRsHIYMmT5a0fLKGeJcwK3ewoiRHxjKpOr9hXZau9f7CVVqMvtWC2ngWrFsEeh8S0YtRZQFT-7XyjE9dNiyKp_Z-4iBUbbqzVnT4GmEmErZXUjmBhmVsMLz5h9y_F3usRT3lg7LxUNamnJuROnYIqmJzf0fYVJq1mcB5hcUipo2SNcILG3xkUikc84VznSGvD7V_qFjOHVPtOEX02JH9M4ymb3iZtZSE9dDr2RkwTU7StoKgM-x195bBULpwms8weJO-kx5I6UrY_lmWl0SFqYN
&x-client-SKU=ID_NETSTANDARD2_0
&x-client-ver=5.4.0.0

             */
            NameValueCollection values;
            if (HttpMethods.IsGet(Request.Method))
            {
                values = Request.Query.AsNameValueCollection();
            }
            else if (HttpMethods.IsPost(Request.Method))
            {
                if (!Request.HasFormContentType)
                {
                    return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
                }

                values = Request.Form.AsNameValueCollection();
            }
            else
            {
                return new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }

            var result = await ProcessAuthorizeRequestAsync(values);
            var idTokenAuthorizationRequest = new IdTokenAuthorizationRequest
            {
                client_id = values.Get(OidcConstants.AuthorizeRequest.ClientId),
                client_secret = values.Get("client_secret"),
                nonce = values.Get(OidcConstants.AuthorizeRequest.Nonce),
                response_mode = values.Get(OidcConstants.AuthorizeRequest.ResponseMode),
                redirect_uri = values.Get(OidcConstants.AuthorizeRequest.RedirectUri),
                response_type = values.Get(OidcConstants.AuthorizeRequest.ResponseType)
            };
            this.SetJsonCookie( "idTokenAuthorizationRequest", idTokenAuthorizationRequest, 30);
          

            return result;
        }
        internal async Task<IActionResult> ProcessAuthorizeRequestAsync(NameValueCollection parameters)
        {
            var result = await _authorizeRequestValidator.ValidateAsync(parameters);
            if (result.IsError)
            {
                return BadRequest(result.ErrorDescription);
            }
            return Ok();
        }
        // PUT: api/OIDC/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
