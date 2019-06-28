using IdentityModel;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OIDC.ReferenceWebClient
{
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
    /// <summary>
    ///  Authorize endpoint request validator.
    /// </summary>
    public interface IAuthorizeRequestValidator
    {
        /// <summary>
        ///  Validates authorize request parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        Task<AuthorizeRequestValidationResult> ValidateAsync(NameValueCollection parameters);
    }

    public class AuthorizeRequestValidator : IAuthorizeRequestValidator
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
        private ILogger<AuthorizeRequestValidator> _logger;

        public async Task<AuthorizeRequestValidationResult> ValidateAsync(NameValueCollection parameters)
        {
            //////////////////////////////////////////////////////////
            // response_type must be present and supported
            //////////////////////////////////////////////////////////

            var responseType = parameters.Get(OidcConstants.AuthorizeRequest.ResponseType);


            if (responseType.IsMissing())
            {
                _logger.LogError("Missing response_type");
                return Invalid(OidcConstants.AuthorizeErrors.UnsupportedResponseType, "Missing response_type");
            }
            if (!SupportedResponseTypes.Contains(responseType, _responseTypeEqualityComparer))
            {
                _logger.LogError("Response type not supported", responseType);
                return Invalid(OidcConstants.AuthorizeErrors.UnsupportedResponseType, "Response type not supported");
            }
            var rt = SupportedResponseTypes.First(
              supportedResponseType => _responseTypeEqualityComparer.Equals(supportedResponseType, responseType));


            return Valid();
        }
        private AuthorizeRequestValidationResult Invalid( string error = OidcConstants.AuthorizeErrors.InvalidRequest, string description = null)
        {
            return new AuthorizeRequestValidationResult( error, description);
        }

        private AuthorizeRequestValidationResult Valid()
        {
            return new AuthorizeRequestValidationResult();
        }
    }
}
