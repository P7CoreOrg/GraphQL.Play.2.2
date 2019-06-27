using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIDC.ReferenceWebClient.Controllers
{
    [ApiController]
    [Route("")]
    public class OIDCController : ControllerBase
    {
        // GET: api/OIDC
        [HttpGet]
        [Route(".well-known/openid-configuration")]
        public Dictionary<string, object> Get()
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
        public void Post([FromBody] string value)
        {
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
