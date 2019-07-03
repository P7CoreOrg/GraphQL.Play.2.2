using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCPipeline.Core.AuthorizationEndpoint
{
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
}
