using IdentityModel;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCPipeline.Core.AuthorizationEndpoint
{
    internal class AuthorizeRequestValidator : IAuthorizeRequestValidator
    {
        public AuthorizeRequestValidator(ILogger<AuthorizeRequestValidator> logger)
        {
            _logger = logger;
        }
        private readonly ResponseTypeEqualityComparer
          _responseTypeEqualityComparer = new ResponseTypeEqualityComparer();
        public static readonly List<string> SupportedResponseTypes = new List<string>
        {
            OidcConstants.ResponseTypes.IdToken,
            OidcConstants.ResponseTypes.IdTokenToken
        };
        public static readonly List<string> SupportedResponseModes = new List<string>
        {
            OidcConstants.ResponseModes.FormPost,
            OidcConstants.ResponseModes.Query,
            OidcConstants.ResponseModes.Fragment
        };
        private ILogger<AuthorizeRequestValidator> _logger;

        public async Task<AuthorizeRequestValidationResult> ValidateAsync(NameValueCollection parameters)
        {
            //////////////////////////////////////////////////////////
            // response_type must be present and supported
            //////////////////////////////////////////////////////////

            var responseType = parameters.Get(OidcConstants.AuthorizeRequest.ResponseType);


            if (responseType.IsMissing())
            {
                _logger.LogError($"Missing {OidcConstants.AuthorizeRequest.ResponseType}");
                return Invalid(OidcConstants.AuthorizeErrors.UnsupportedResponseType, $"Missing {OidcConstants.AuthorizeRequest.ResponseType}");
            }
            if (!SupportedResponseTypes.Contains(responseType, _responseTypeEqualityComparer))
            {
                _logger.LogError("Response type not supported", responseType);
                return Invalid(OidcConstants.AuthorizeErrors.UnsupportedResponseType, "Response type not supported");
            }
            var rt = SupportedResponseTypes.First(
              supportedResponseType => _responseTypeEqualityComparer.Equals(supportedResponseType, responseType));

            //////////////////////////////////////////////////////////
            // client_id must be present and supported
            //////////////////////////////////////////////////////////

            var clientId = parameters.Get(OidcConstants.AuthorizeRequest.ClientId);


            if (clientId.IsMissing())
            {
                _logger.LogError($"Missing {OidcConstants.AuthorizeRequest.ClientId}");
                return Invalid(OidcConstants.AuthorizeErrors.UnauthorizedClient, $"Missing {OidcConstants.AuthorizeRequest.ClientId}");
            }

            //////////////////////////////////////////////////////////
            // redirect_uri must be present, and a valid uri
            //////////////////////////////////////////////////////////
            var redirectUri = parameters.Get(OidcConstants.AuthorizeRequest.RedirectUri);

            if (redirectUri.IsMissing())
            {
                _logger.LogError($"Missing {OidcConstants.AuthorizeRequest.RedirectUri}");
                return Invalid(OidcConstants.AuthorizeRequest.RedirectUri, $"Missing {OidcConstants.AuthorizeRequest.RedirectUri}");
            }

            //////////////////////////////////////////////////////////
            // check response_mode parameter and set response_mode
            //////////////////////////////////////////////////////////

            // check if response_mode parameter is present and valid
            var responseMode = parameters.Get(OidcConstants.AuthorizeRequest.ResponseMode);
            if (responseMode.IsPresent())
            {
                if (!SupportedResponseModes.Contains(responseMode))
                {
                    _logger.LogError("Unsupported response_mode", responseMode);
                    return Invalid(OidcConstants.AuthorizeErrors.UnsupportedResponseType, description: "Invalid response_mode");
                }
            }

            return Valid();
        }
        private AuthorizeRequestValidationResult Invalid(string error = OidcConstants.AuthorizeErrors.InvalidRequest, string description = null)
        {
            return new AuthorizeRequestValidationResult(error, description);
        }

        private AuthorizeRequestValidationResult Valid()
        {
            return new AuthorizeRequestValidationResult();
        }
    }
}
